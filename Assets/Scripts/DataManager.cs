using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
	[SerializeField] List<Color> colors = new List<Color>();
	[SerializeField] Color gameOverColor = new Color();

	public Color GetColor()
	{
		return colors[Random.Range(0, colors.Count)];
	}
	public Color GetGameOverColor() => gameOverColor;



}
