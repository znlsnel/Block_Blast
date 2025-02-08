using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
	float width = 5.0f;

	List<float> points = new List<float>();

	private void Awake()
	{
		SpriteRenderer rdr = GetComponent<SpriteRenderer>();
		if (rdr != null && rdr.sprite != null)
			width = rdr.sprite.bounds.size.x;
	}
}
