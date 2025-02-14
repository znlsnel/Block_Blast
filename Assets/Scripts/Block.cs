using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Block : MonoBehaviour
{
	[SerializeField] GameObject blockPiece;

	[NonSerialized] public List<GameObject> pieces = new List<GameObject>();
	[NonSerialized] public UnityEvent onRelase = new UnityEvent();
	[NonSerialized] public UnityEngine.Color blockColor;

	List<HashSet<(int, int)>> tiles = new List<HashSet<(int, int)>>();
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
			pieces.Add(go);
			go.GetComponent<Tile>().GetSpriteRenderer().sortingOrder++;
		}
		blockColor = GameManager.instance.GetColor();

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
		if (tiles[rot] == null)
			return true;

		return Board.instance.CanPlaceTileOnBoard(tiles[rot]);
	}

	void InitTilePos()
	{
		rot = UnityEngine.Random.Range(0, 4);
		transform.rotation = Quaternion.Euler(0f, 0f, rot * -90f); 

		tiles.Add(new HashSet<(int, int)>());
		foreach (GameObject child in pieces)
		{
			Vector3 pos = child.transform.localPosition;
			tiles[0].Add(((int)pos.y, (int)pos.x)); 
		}

		for (int i = 1; i < 4; i++)
		{
			tiles.Add(new HashSet<(int, int)>());
			foreach (var tile in tiles[i-1])
				tiles[i].Add((-tile.Item2, tile.Item1));
		}
	}

	public void SetColor()
	{
		foreach (GameObject piece in pieces)
			piece.GetComponent<Tile>().GetSpriteRenderer().color = blockColor;
	}

	// (-x, y)

	public void RelaseBlock()
	{
		onRelase?.Invoke();
		gameObject.SetActive(false);
		Destroy(gameObject);
	}

	public void OnPlayerDeck()
	{
		transform.position = origin;
		//StartCoroutine(ResizeBlock(originSize, 0.1f));
		transform.localScale = new Vector3(originSize, originSize, originSize);
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
