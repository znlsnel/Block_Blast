using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
	public static BlockSpawner instance;

	[SerializeField] List<GameObject> blocksPrefabs= new List<GameObject>();
	[SerializeField] int blockCount = 3;

	List<Vector3> points = new List<Vector3>();
	HashSet<Block> myBlocks = new HashSet<Block>();	
	float width = 3.8f;

	private void Awake()
	{
		instance = this;
		float offset = width / (blockCount + 1);
		Vector3 pos = transform.position + new Vector3(-width/2, 0, 0);

		for (int i = 0; i < blockCount; i++)
		{
			pos += new Vector3(offset, 0, 0); 
			points.Add(pos); 
		}
	}
	private void Start()
	{
		SpawnBlock();
	} 
	void SpawnBlock()
	{
		foreach (Vector3 pos in points)
		{
			var go = Instantiate(blocksPrefabs[UnityEngine.Random.Range(0, blocksPrefabs.Count)]);
			go.transform.position = pos;
			go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

			Block block = go.GetComponent<Block>();
			myBlocks.Add(block);
			block.onRelase.AddListener(() => { myBlocks.Remove(block); });
		}
		CanContinueGame();
	}



	public void UseBlock()
	{
		if (myBlocks.Count == 0)
			SpawnBlock();

		CanContinueGame();
	}

	void CanContinueGame()
	{
		bool flag = false;
		foreach (Block block in myBlocks)
		{
			if (block.CanPlaceTileOnBoard())
			{
				flag = true;
				break;
			}
		}
		if (flag == false)
		{
			Debug.Log("게임 실패!");
			Board.instance.GameOver();
		}
	}


}
