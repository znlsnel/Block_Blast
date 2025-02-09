using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
	public static PlayerDeck instance;
	[SerializeField] int blockCount = 3;
	[NonSerialized] public int curBlocks = 0;
	[SerializeField] List<GameObject> blocks= new List<GameObject>();

	float width = 3.8f;

	List<Vector3> points = new List<Vector3>();



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

	public void UseBlock()
	{
		curBlocks -= 1;
		if (curBlocks == 0)
		{
			SpawnBlock();
		}
	}

	void SpawnBlock()
	{
		foreach (Vector3 pos in points)
		{
			//var go = Instantiate(blocks[UnityEngine.Random.Range(0, blocks.Count)]);
			var go = Instantiate(blocks[1]);
			go.transform.position = pos;
			go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		}
		curBlocks = points.Count;
	}
}
