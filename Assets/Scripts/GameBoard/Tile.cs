using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Animator anim;
    [SerializeField] SpriteRenderer sr;

    public bool isActiveTile = false;

    void Awake()
    {
		anim = GetComponent<Animator>();
		isActiveTile = false;
	}
	public void HideTile() => gameObject.SetActive(false);
	public void IncreaseSortingOrder() => sr.sortingOrder++;
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
		temp.a = 0.3f;
		sr.color = temp;
		gameObject.SetActive(fade); 
	}

	public void GameOver(Color color)
	{
		if (isActiveTile == false)
			return;

		PushTile();
		SetColor(color);
	}
}
