using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Notifications.iOS;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;
using Object = UnityEngine.Object;

public class Board : MonoBehaviour
{
	public static Board instance;
		
	[SerializeField] GameObject tile;
	[SerializeField] GameObject blockPiecePrefab;
	[SerializeField] float length = 4.1f;
	[SerializeField] int tileSize = 8;

	[Space(10)]
	[SerializeField] GameObject gameOverUI;
	[SerializeField] Object gameOverScene;

	int combo = 1;
	int placeBlockCnt = 0;

	Vector3[,] gameBoard;
	Tile[,] tiles;
	Block hoveringBlock;

	private void Awake()
	{
		gameOverUI.SetActive(false);
		gameBoard = new Vector3[tileSize, tileSize];
		tiles = new Tile[tileSize, tileSize];
		instance = this;
	}
 
	void Start()
	{
		SetTiles();
		StartCoroutine(CheckHover());
	}
	void SetTiles()
    {
		float size = GetTileSize();

		Vector3 startPos = transform.position + new Vector3(-length / 2 + size/2, -length / 2+size/2, 0.0f);

		for (int i = 0; i < tileSize; i++)
		{
			for (int j = 0; j < tileSize; j++)
			{
				gameBoard[i, j] = startPos + new Vector3(j * size, i * size, 0.0f);

				var go = Instantiate<GameObject>(tile);
				go.transform.position = gameBoard[i, j];
				go.transform.localScale = new Vector3(size, size, size);
				go.transform.SetParent(transform, false);

				var go2 = Instantiate<GameObject>(blockPiecePrefab);
				go2.transform.position = gameBoard[i, j];
				go2.transform.localScale = new Vector3(size, size, size);
				go2.transform.SetParent(transform, false);

				tiles[i, j] = go2.GetComponent<Tile>();
				tiles[i, j].gameObject.SetActive(false);

			}
		}
    }
	int GetComboScore()
	{
		if (placeBlockCnt > 2)
		{
			combo = 1;
			placeBlockCnt = 0;
		}

		int ret = 10 * combo++;
		return ret;
	}

	public float GetTileSize() => length / tileSize;
	bool isVisited(int y, int x) => tiles[y, x].isActiveTile;
	public bool CanPlaceTile(HashSet<(int,  int)> hash)
	{
		for (int i = 0; i < tileSize; i++)
		{
			for (int j = 0; j < tileSize; j++)
			{
				bool canPlace = true;
				foreach(var (dy, dx) in hash)
				{
					var (y, x) = (i + dy, j + dx);
					if (y < 0 || x < 0 || y >= tileSize || x >= tileSize || isVisited(y, x))
					{
						canPlace = false; 
						break;
					}
				} 
				if (canPlace) 
					return true;
			}
		}
		return false;
	}
	public bool CanPlaceTile(out HashSet<(int, int)> tilsPos,  Block block)
	{
		tilsPos = new HashSet<(int, int)>();
		foreach (GameObject piece in block.tiles)
		{
			GetClosestTile(piece.transform.position, out int y, out int x);
			if (y == -1 || x == -1)
				continue;

			tilsPos.Add((y, x));
		}

		return tilsPos.Count == block.tiles.Count;
	}
	public bool PutBlock(Block block)
	{
		bool ret = CanPlaceTile(out HashSet<(int, int)> hash, block);
		if (ret) 
		{
			block.RelaseBlock(); 

			foreach(var pos in hash)
			{
				int y = pos.Item1;
				int x = pos.Item2;

				tiles[y, x].PushTile();
				tiles[y, x].SetColor(block.blockColor);
				
			}
			DataManager.Instance.AddScore(hash.Count());
			placeBlockCnt++;

			Utils.Instance.SetTimer(()=> {
				CheckBingo();
				BlockSpawner.instance.UseBlock(); 
			} 
			, 0.1f);
		}
		return ret;
	}

	void CheckBingo()
	{
		List<int> successY = new List<int>();
		List<int> successX = new List<int>();

		for (int y = 0; y < tiles.GetLength(0); y++)
		{
			bool flag = true;
			for (int x = 0; x < tiles.GetLength(1); x++)
			{
				if (isVisited(y, x) == false)
				{
					flag = false;
					break;
				}
			}
			if (flag)
				successX.Add(y);
		}


		for (int x = 0; x < tiles.GetLength(1); x++)
		{
			bool flag = true;
			for (int y = 0; y < tiles.GetLength(0); y++)
			{
				if (isVisited(y, x) == false)
				{
					flag = false;
					break;
				}
			}
			if (flag)
				successY.Add(x);
		}

		foreach (int idx in successY)
		{
			for (int i = 0; i < tileSize; i++)
				tiles[i, idx].PopTile();
			DataManager.Instance.AddScore(GetComboScore());
		}

		foreach (int idx in successX)
		{
			for (int i = 0; i < tileSize; i++)
				tiles[idx, i].PopTile();
			DataManager.Instance.AddScore(GetComboScore());
		}

	}
	void GetClosestTile(Vector3 pos, out int y, out int x)
	{
		float size = GetTileSize();
		Vector3 target = pos - (gameBoard[0, 0] - new Vector3(size/2, size/2, 0));
		Vector3 max = (gameBoard[tileSize - 1, tileSize - 1] - gameBoard[0, 0]) + new Vector3(size, size, 0);
		y = -1;
		x = -1;
	
		if (target.x < 0 || target.y < 0 || target.x > max.x  || target.y > max.y)
			return;

		y = (int)(target.y / size);
		x = (int)(target.x / size);
		if (isVisited(y, x) == true)
		{
			y = -1;
			x = -1;
		}
	}

	public void Hovering(HashSet<(int, int)> hash)
	{
		foreach (var (y, x) in hash)
		{
			tiles[y, x].SetColor(hoveringBlock.blockColor);
			tiles[y, x].FadeMode();
		}
	}

	public void SetHoverBlock(Block block)
	{
		hoveringBlock = block; 
	}

	IEnumerator CheckHover()
	{
		HashSet<(int, int)> prevIdxs = null; 
		var (prevY, prevX) = (-10, -10);

		while (true)
		{
			if (hoveringBlock != null && CanPlaceTile(out var curIdxs, hoveringBlock))
			{
				GetClosestTile(hoveringBlock.tiles[0].transform.position, out int y, out int x);
				var (curY, curX) = (y, x);

				if (prevY != curY || prevX != curX)
				{
					Hovering(curIdxs); 
					prevY = curY;
					prevX = curX;

					// 중복된 인덱스 제거
					if (curIdxs != null && prevIdxs != null)
					{
						foreach (var curIdx in curIdxs)
						prevIdxs.Remove(curIdx);
					} 

					if (prevIdxs != null)
					{
						foreach (var h in prevIdxs)
						{
							(y, x) = h;
							tiles[y, x].FadeMode(false);
						}
					}

					prevIdxs = curIdxs;
				} 
			}
			else
			{
				(prevY, prevX) = (-10, -10);
				if (prevIdxs != null)
				{
					foreach (var h in prevIdxs)
					{
						var (y, x) = h;
						tiles[y, x].FadeMode(false);
					}
					prevIdxs = null;
				}
			}

			yield return null;
		}
	}


	public void GameOver()
	{
		StartCoroutine(RunGameOver());
	}

	int[] dy = { -1, 0, -1 };
	int[] dx = { 0, 1, 1}; 
	IEnumerator RunGameOver()
	{
		yield return new WaitForSeconds(2.0f);  
		gameOverUI.SetActive(true);
		Color color = DataManager.Instance.GetGameOverColor();

		HashSet<(int, int)> visited = new HashSet<(int, int)>();
		Queue<(int, int)> q = new Queue<(int, int)> ();
		 
		q.Enqueue((tileSize - 1, 0));
		visited.Add((tileSize - 1, 0));

		 
		while (q.Count > 0)
		{
			var (curY, curX) = q.Dequeue();
			tiles[curY, curX].GameOver(color);

			for (int i = 0; i < 3; i++)
			{
				var (nxtY, nxtX) = (curY + dy[i], curX + dx[i]);

				if (nxtY < 0 || nxtX < 0 || nxtX >= tiles.GetLength(1) || visited.Contains((nxtY, nxtX)))
					continue;

				q.Enqueue((nxtY, nxtX));
				visited.Add((nxtY, nxtX));
			}

			yield return new WaitForSeconds(0.03f); 
		}

		yield return new WaitForSeconds(1.0f);
		SceneManager.LoadScene(gameOverScene.name);
	}

}
