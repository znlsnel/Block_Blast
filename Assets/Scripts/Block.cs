using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Block : MonoBehaviour
{
	[SerializeField] GameObject blockPiece;

	Vector3 origin;

	private void Start()
	{
		SetRotation();
		origin = transform.position;

	}


	public void SetRotation()
	{
		int rand = UnityEngine.Random.Range(0, 4);
		float rot = rand == 1 ? 90f : rand == 2 ? -90f : rand == 3 ? 180f : 0f; 
		transform.rotation = Quaternion.Euler(0f, 0f, rot);
	}

	public void InitBlock()
	{
		
	}

}
