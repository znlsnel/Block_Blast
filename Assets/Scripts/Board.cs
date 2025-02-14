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
using static UnityEditor.PlayerSettings;

public class Board : MonoBehaviour
{
	public static Board instance;
		
	[SerializeField] GameObject tile;
	[SerializeField] GameObject blockPiecePrefab;

	[SerializeField] float length = 4.1f;
	[SerializeField] int tileSize = 8;

	Vector3[,] gameBoard;
	Tile[,] blockPieces;

    public float GetTileSize() => length / tileSize;
	private void Awake()
	{
		gameBoard = new Vector3[tileSize, tileSize];
		blockPieces = new Tile[tileSize, tileSize];

		instance = this;
	}

	void Start()
	{
		SetTiles();
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

				blockPieces[i, j] = go2.GetComponent<Tile>();
				blockPieces[i, j].gameObject.SetActive(false);

			}
		}
    }

	bool isVisited(int y, int x)
	{
		return blockPieces[y, x].isActiveTile;
	}

	// 회전값 반영 안됨
	public bool CanPlaceTileOnBoard(HashSet<(int,  int)> hash)
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
	public bool CanPlaceTileHere(out HashSet<(int, int)> tilsPos,  Block block)
	{
		tilsPos = new HashSet<(int, int)>();
		foreach (GameObject piece in block.pieces)
		{
			GetClosestTile(piece.transform.position, out int y, out int x);
			if (y == -1 || x == -1)
				continue;

			tilsPos.Add((y, x));
		}

		return tilsPos.Count == block.pieces.Count;
	}

	public bool PutBlock(Block block)
	{
		bool ret = CanPlaceTileHere(out HashSet<(int, int)> hash, block);
		if (ret) 
		{
			block.RelaseBlock();

			foreach(var pos in hash)
			{
				int y = pos.Item1;
				int x = pos.Item2;

				blockPieces[y, x].PushTile();
				blockPieces[y, x].GetSpriteRenderer().color = block.blockColor;
			}
			Utils.instance.SetTimer(()=> {
				CheckLine();
				PlayManager.instance.UseBlock(); 
			} 
			, 0.1f);
			
			
		}
		

		return ret;
	}
	
	void CheckLine()
	{
		List<int> successY = new List<int>();
		List<int> successX = new List<int>();

		for (int y = 0; y < blockPieces.GetLength(0); y++)
		{
			bool flag = true;
			for (int x = 0; x < blockPieces.GetLength(1); x++)
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


		for (int x = 0; x < blockPieces.GetLength(1); x++)
		{
			bool flag = true;
			for (int y = 0; y < blockPieces.GetLength(0); y++)
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
			for (int i = 0; i < tileSize; i++)
				blockPieces[i, idx].PopTile();


		foreach (int idx in successX)
			for (int i = 0; i < tileSize; i++)
				blockPieces[idx, i].PopTile();

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


	private void OnTriggerStay2D(Collider2D collision)
	{

	}


}
