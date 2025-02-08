using System;
using UnityEngine;
using UnityEngine.UI;

public class BlockPiece : MonoBehaviour
{
        Block parent;

    void Start()
    {
                Transform p = gameObject.transform.parent;

                while (p.parent != null)
                        p = p.parent;

		if (p != null)
			parent = p.GetComponent<Block>();
	}




}
