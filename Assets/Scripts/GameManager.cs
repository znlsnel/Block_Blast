using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public List<Color> colors = new List<Color>();
	private void Awake()
	{
		instance = this;
	}

	public Color GetColor()
	{
		return colors[Random.Range(0, colors.Count)];
	}

	
}
