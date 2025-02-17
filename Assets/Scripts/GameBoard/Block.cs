using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class Block : MonoBehaviour
{
	[SerializeField] GameObject blockPiece;

	[NonSerialized] public List<GameObject> tiles = new List<GameObject>();
	[NonSerialized] public UnityEvent onRelase = new UnityEvent();
	[NonSerialized] public UnityEngine.Color blockColor;

	List<HashSet<(int, int)>> tileLoc = new List<HashSet<(int, int)>>();
	Vector3 origin;
	float originSize;
	int rot = 0;

	private void Awake()
	{
		Transform center = transform.GetChild(0);
		int childCnt = center.childCount;
		for (int i = 0; i < childCnt; i++)
		{
			GameObject go = center.GetChild(i).gameObject;
			tiles.Add(go);
			go.GetComponent<Tile>().IncreaseSortingOrder();
		}
		blockColor = DataManager.Instance.GetColor();

		InitTilePos();
		SetColor(); 
	}

	private void Start()
	{
		origin = transform.position; 
		originSize = transform.localScale.x;
	}

	public bool CanPlaceTileOnBoard()
	{
		if (tileLoc[rot] == null)
			return true;

		return Board.instance.CanPlaceTile(tileLoc[rot]);
	}

	public HashSet<(int, int)> GetTile() => tileLoc[rot];
	void InitTilePos()
	{
		rot = UnityEngine.Random.Range(0, 4);
		transform.rotation = Quaternion.Euler(0f, 0f, rot * -90f); 

		tileLoc.Add(new HashSet<(int, int)>());
		foreach (GameObject child in tiles)
		{
			Vector3 pos = child.transform.localPosition;
			tileLoc[0].Add(((int)pos.y, (int)pos.x)); 
		}

		for (int i = 1; i < 4; i++)
		{
			tileLoc.Add(new HashSet<(int, int)>());
			foreach (var tile in tileLoc[i-1])
				tileLoc[i].Add((-tile.Item2, tile.Item1));
		}
	}

	public void SetColor()
	{
		foreach (GameObject piece in tiles)
			piece.GetComponent<Tile>().SetColor(blockColor);
	}

	public void SetLockColor()
	{
		Color color = DataManager.Instance.GetGameOverColor();
		foreach (GameObject piece in tiles)
			piece.GetComponent<Tile>().SetColor(color);
	}

	public void SetOriginColor()
	{
		foreach (GameObject piece in tiles)
			piece.GetComponent<Tile>().SetColor(blockColor);
	}

	public void RelaseBlock()
	{
		onRelase?.Invoke();
		gameObject.SetActive(false);
		Destroy(gameObject);
	}

	Coroutine resize;
	public void ReturnPlayerDeck()
	{
		if (resize != null)
			StopCoroutine(resize);

		transform.position = origin;
		//StartCoroutine(ResizeBlock(originSize, 0.1f));
		transform.localScale = new Vector3(originSize, originSize, originSize);
	} 

	public void OnDragMode()
	{
		if (resize != null)
			StopCoroutine(resize);
		resize = StartCoroutine(ResizeBlock(Board.instance.GetTileSize(), 0.2f));
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
		resize = null;
	}
}
