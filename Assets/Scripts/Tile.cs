using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Animator anim;
    [SerializeField] SpriteRenderer sr;
    void Awake()
    {
		anim = GetComponent<Animator>();
	}

    public void PopTile()
    { 
	    anim.SetTrigger("pop");
	}

	public void PushTile()
    {
        gameObject.SetActive(true);
		anim.SetTrigger("push");
	}

    public void HideTile()
    {
        gameObject.SetActive(false);
    }


	public SpriteRenderer GetSpriteRenderer() => sr;
}
