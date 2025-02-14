using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Animator anim;
    [SerializeField] SpriteRenderer sr;

    public bool isActiveTile = false;
	Coroutine diableTimer;
    void Awake()
    {
		anim = GetComponent<Animator>();
		isActiveTile = false;
	}
	private void Start()
	{
	}

	public void PopTile()
    {
		isActiveTile = false;
	    anim.SetTrigger("pop");
	}

	public void PushTile()
    {
		if (diableTimer != null)
			StopCoroutine(diableTimer);
		isActiveTile = true;

		gameObject.SetActive(true);
		anim.SetTrigger("push");

	}

    public void HideTile()
    {
		if (diableTimer != null)
			StopCoroutine(diableTimer);

		gameObject.SetActive(false);
	}

	public void OnFadeMode()
	{
		if (diableTimer != null)
			StopCoroutine(diableTimer);

		Color temp = sr.color;
		temp.a = 0.3f;
		sr.color = temp;
		gameObject.SetActive(true);
		diableTimer = StartCoroutine(DisableTile(0.1f));
	}

	public void IncreaseSortingOrder()
	{
		sr.sortingOrder++;
	}

	public void SetColor(Color color)
	{
		sr.color = color;
	}

	IEnumerator DisableTile(float time)
	{
		yield return new WaitForSeconds(time);
		diableTimer = null;
		gameObject.SetActive(false);
	}

	
}
