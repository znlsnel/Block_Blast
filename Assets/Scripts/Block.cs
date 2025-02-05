using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Block : MonoBehaviour
{
	[SerializeField] GameObject blockPiece;

	private void Start()
	{

	}


	public void CreateBlock(float size)
	{
		int[][] shape = new int[2][];
		
		// 위로 90도 아래로 90도, 180도 회전
		
		for (int i = 0; i < shape.Length; i++)
		{
			for (int j = 0; j < shape[i].Length; j++)
			{
				if (shape[i][j] == 0)
					continue;

				var go = Instantiate<GameObject>(blockPiece);
				go.transform.position = transform.position + new Vector3(size * j, size * i, 0);
				go.transform.localScale = new Vector3(size, size, size);
				go.transform.SetParent(transform, false);
			}
		}
	}



}
