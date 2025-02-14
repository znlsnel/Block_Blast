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
		isActiveTile = true;

		gameObject.SetActive(true);
		anim.SetTrigger("push");

	}

    public void HideTile()
    {
        gameObject.SetActive(false);
	}


	public SpriteRenderer GetSpriteRenderer() => sr;
}
