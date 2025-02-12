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
        anim.SetBool("pop", true);
        PlayManager.instance.SetTimer(() => { 
            gameObject.SetActive(false);
            anim.SetBool("pop", false); 
        
        }, 1.0f);
    }
    
    public void PushTile()
    {
        gameObject.SetActive(true);
		anim.SetBool("push", true);
		PlayManager.instance.SetTimer(() => { 
            anim.SetBool("push", false); 
		}, 1.0f);
	}

    public SpriteRenderer GetSpriteRenderer() => sr;
}
