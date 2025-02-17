using NUnit.Framework.Constraints;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    Animator anim; 
    [SerializeField] Canvas canvas;
    [SerializeField] Image sr;

    [NonSerialized] public bool isActiveTile = false;
	Color myColor;
    void Awake()
    {
		anim = GetComponent<Animator>();
		isActiveTile = false;
	}
	public void HideTile() => gameObject.SetActive(false);
	public void IncreaseSortingOrder()
	{
		 	canvas.sortingOrder++;
	}
	public void SetColor(Color color) => sr.color = color;

	public void PopTile()
    {
		isActiveTile = false;
	    anim.SetTrigger("pop");
	}

	public void PushTile()
    {
		isActiveTile = true;
		gameObject.SetActive(true);
		anim.SetTrigger("push");
	}

	public void FadeMode(bool fade = true)
	{
		if (isActiveTile)
			return;
		 
		Color temp = sr.color;
		temp.a = 0.6f;
		sr.color = temp;
		gameObject.SetActive(fade); 
	}

	public void GameOver(Color color)
	{
		PushTile();
		SetColor(color);
	}
}
