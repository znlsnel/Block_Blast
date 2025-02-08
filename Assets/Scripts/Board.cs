using NUnit.Framework;
using System.Runtime.InteropServices;
using UnityEngine;

public class Board : MonoBehaviour
{
        public static Board board;
        [SerializeField] GameObject tile;
        [SerializeField] float length = 4.1f;
        [SerializeField] int tileSize = 8;

        public float GetTileSize() => length / tileSize;
	private void Awake()
	{
                board = this;
	}

	void Start()
        {
                SetTiles();

        }


        void SetTiles()
        {
                float size = length / tileSize;
                Vector3 startPos = transform.position + new Vector3(-length / 2 + size/2, -length / 2+size/2, 0.0f);

		for (int i = 0; i < tileSize; i++)
                {
                        for (int  j = 0; j < tileSize; j++ )
                        {
                                var go = Instantiate<GameObject>(tile);
                                go.transform.position = startPos + new Vector3(j * size, i * size, 0.0f);
                                go.transform.localScale = new Vector3(size, size, size);
				go.transform.SetParent(transform, false);

			}
                }

        }
}
