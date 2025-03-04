using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DataManager : Singleton<DataManager>
{
	[SerializeField] List<Color> colors = new List<Color>();
	[SerializeField] Color gameOverColor = new Color();
	public UnityEvent onUpdateScore = new UnityEvent();
	public int score { get; set; } = 0;
	public int bestScore { get; set; } = 0;
	public int retryCnt = 0;

	public void InitScore() => score = 0;
	public void AddScore(int num)
	{
		score += num;
		onUpdateScore?.Invoke();
	}
	public void UpdateBestScore() => bestScore = score;

	public Color GetColor()
	{
		return colors[Random.Range(0, colors.Count)];
	}
	public Color GetGameOverColor() => gameOverColor;

}
