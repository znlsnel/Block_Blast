using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Block : MonoBehaviour
{
	[SerializeField] GameObject blockPiece;

	private void Start()
	{
		SetRotation();


	}


	public void SetRotation()
	{
		transform.rotation = Quaternion.Euler(0f, 0f, 90f);

	}



}
