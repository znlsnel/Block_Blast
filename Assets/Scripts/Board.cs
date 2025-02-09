using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        bool[,] visited;
	GameObject[,] blockPieces;

	int[] bingoY;
	int[] bingoX;

        public float GetTileSize() => length / tileSize;
	private void Awake()
	{
                gameBoard = new Vector3[tileSize, tileSize];
		visited = new bool[tileSize, tileSize];
		blockPieces = new GameObject[tileSize, tileSize];
		bingoY = new int[tileSize];
		bingoX = new int[tileSize];

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
                        for (int  j = 0; j < tileSize; j++ )
                        {
				gameBoard[i, j] = startPos + new Vector3(j * size, i * size, 0.0f);
				visited[i, j] = false;

				var go = Instantiate<GameObject>(tile);
                                go.transform.position = gameBoard[i, j];
				go.transform.localScale = new Vector3(size, size, size);
				go.transform.SetParent(transform, false);

				var go2 = Instantiate<GameObject>(blockPiecePrefab);
				go2.transform.position = gameBoard[i, j];
				go2.transform.localScale = new Vector3(size, size, size);
				go2.transform.SetParent(transform, false);

				blockPieces[i, j] = go2;
				blockPieces[i, j].SetActive(false);
				
			}
		}
        }

	public bool PutBlock(Block block)
	{
		HashSet<(int, int)> hash = new HashSet<(int, int)> ();
		foreach (GameObject piece in block.pieces)
		{
			GetClosestTile(piece.transform.position, out int y, out int x);
			if (y == -1 || x == -1)
				continue;

			hash.Add((y, x));
		}

		bool ret = hash.Count == block.pieces.Count;
		if (ret)
		{
			block.gameObject.SetActive(false);

			foreach(var pos in hash)
			{
				int y = pos.Item1;
				int x = pos.Item2;

				blockPieces[y, x].SetActive(true);
				blockPieces[y, x].GetComponent<SpriteRenderer>().color = block.blockColor;

				visited[y, x] = true;

				++bingoY[x];
				++bingoX[y];

				CheckLine(y, x);
			}
			PlayerDeck.instance.UseBlock();
		}

		return ret;
	}

	void CheckLine(int y, int x)
	{
		bool successY = bingoY[x] == tileSize;
		bool successX = bingoX[y] == tileSize;

		if (successY)
		{
			for (int i = 0; i < blockPieces.GetLength(0); i++)
			{
				blockPieces[i, x].SetActive(false);
				visited[i, x] = false;
				--bingoX[i];
			}
			bingoY[x] = 0;
		}

		if (successX)
		{
			for (int i = 0; i < blockPieces.GetLength(1); i++)
			{
				blockPieces[y, i].SetActive(false);
				visited[y, i] = false;
				--bingoY[i];
			}
			bingoX[y] = 0;
		}
	}

	void GetClosestTile(Vector3 pos, out int y, out int x)
	{
		int rows = gameBoard.GetLength(0);
		int cols = gameBoard.GetLength(1);

		// X축과 Y축을 이진 탐색으로 찾기
		int closestRow = BinarySearch(pos.y, true, rows);
		int closestCol = BinarySearch(pos.x, false, cols);

		y = closestRow;
		x = closestCol;
		if (visited[y, x] == true)
		{
			y = -1;
			x = -1;
		}
	}
	private int BinarySearch(float target, bool isRow, int maxIndex)
	{
		int left = 0, right = maxIndex - 1;

		while (left < right)
		{
			int mid = (left + right) / 2;
			float midValue = isRow ? gameBoard[mid, 0].y : gameBoard[0, mid].x;

			if (midValue < target)
				left = mid + 1;
			else
				right = mid;
		}

		// 가장 가까운 인덱스 반환
		if (left > 0)
		{
			float prevValue = isRow ? gameBoard[left - 1, 0].y : gameBoard[0, left - 1].x;
			float currValue = isRow ? gameBoard[left, 0].y : gameBoard[0, left].x;

			if (Mathf.Abs(prevValue - target) < Mathf.Abs(currValue - target))
				return left - 1;
		}

		return left;
	}


	// TODO 
	// 로직을 각 타일에서 하는걸로 변경
	Dictionary<GameObject, GameObject> log = new Dictionary<GameObject, GameObject>();
	private void OnTriggerStay2D(Collider2D collision)
	{
		//StartCoroutine(Timer(() =>
		//{
		//	GetClosestTile(collision.transform.position, out int y, out int x);
		//	blockPieces[y, x].SetActive(true);
		//	log[collision.gameObject] = blockPieces[y, x];
		//}, 0.1f));

	}

	//private void OnTriggerExit2D(Collider2D collision)
	//{

	//	log[collision.gameObject]?.SetActive(false);

	//}


	IEnumerator Timer(Action action, float time)
	{

		yield return new WaitForSeconds(time);
		action?.Invoke();	
	}

}
