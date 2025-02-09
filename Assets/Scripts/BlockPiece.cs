using System;
using UnityEngine;
using UnityEngine.UI;

public class BlockPiece : MonoBehaviour
{
        Block parent;

    void Start()
    {
                Transform p = gameObject.transform.parent;

                if (p == null)
                        return;

                while (p.parent != null)
                        p = p.parent;

		if (p != null)
			parent = p.GetComponent<Block>();
                parent?.pieces.Add(this);
	}




}
