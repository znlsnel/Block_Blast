using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Block : MonoBehaviour
{
	[SerializeField] GameObject blockPiece;

	Vector3 origin;
	float originSize;
	[NonSerialized] public List<GameObject> pieces = new List<GameObject>();
	public UnityEngine.Color blockColor;

	private void Awake()
	{
		Transform center = transform.GetChild(0);
		int childCnt = center.childCount;
		for (int i = 0; i < childCnt; i++)
		{
			GameObject go = center.GetChild(i).gameObject;
			pieces.Add(go);
			go.GetComponent<SpriteRenderer>().sortingOrder++;
		}
		blockColor = GameManager.instance.GetColor();
	}

	private void Start()
	{
		SetRotation();
		origin = transform.position;
		originSize = transform.localScale.x;
		SetColor();
	}

	public void SetColor()
	{
		
		foreach (GameObject piece in pieces)
			piece.GetComponent<SpriteRenderer>().color = blockColor;
		

	}

	public void SetRotation()
	{
		int rand = UnityEngine.Random.Range(0, 4);
		float rot = rand == 1 ? 90f : rand == 2 ? -90f : rand == 3 ? 180f : 0f; 
		transform.rotation = Quaternion.Euler(0f, 0f, rot);
	}



	public void OnPlayerDeck()
	{
		transform.position = origin;
		StartCoroutine(ResizeBlock(originSize, 0.1f));   
	} 

	public void OnDragMode()
	{
		StartCoroutine(ResizeBlock(Board.instance.GetTileSize(), 0.2f));
	}

	IEnumerator ResizeBlock(float size, float time)
	{
	// 블록의 Transform 가져오기
		Vector3 startScale = transform.localScale; // 현재 크기 저장
		Vector3 targetScale = new Vector3(size, size, size); // 목표 크기 설정
		float elapsedTime = 0f;

		while (elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / time;
			transform.localScale = Vector3.Lerp(startScale, targetScale, t);
			yield return null; // 다음 프레임까지 대기
		} 

		transform.localScale = targetScale; // 정확한 크기로 설정
	}

}
