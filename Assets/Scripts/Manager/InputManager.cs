using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	[Header("Asset")]
	public InputActionAsset actionAsset;

	[Header("Action")]
	public InputActionReference clickAction;
	public InputActionReference handAction;

	Block myBlock = null;
	Vector3 handPos;
	void Start()
	{
		actionAsset.Enable();

		clickAction.action.performed += ClickScreen;
		clickAction.action.canceled += RelaseScreen; 
		handAction.action.performed += MoveHandPos;
	}

	void ClickScreen(InputAction.CallbackContext obj) // Action이 실행될 때
	{

		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

		if (hit.collider != null && hit.collider.GetComponent<Block>() != null) // 오브젝트가 감지된 경우
		{
			Debug.Log("Clicked Object: " + hit.collider.gameObject.name);
			myBlock = hit.collider.GetComponent<Block>();
			myBlock.OnDragMode();
			Board.instance.SetHoverBlock(myBlock);
		}

	}


	void RelaseScreen(InputAction.CallbackContext obj) // Action이 실행될 때
	{
		if (myBlock != null && Board.instance.PutBlock(myBlock) == false)
			myBlock.ReturnPlayerDeck();
		 
		myBlock = null;
		Board.instance.SetHoverBlock(myBlock);
	}



	void MoveHandPos(InputAction.CallbackContext obj) // Action이 실행될 때
	{
		handPos = Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>());
		if (myBlock != null)
		{
			Vector3 pos = handPos;
			pos.z = 0;
			pos.y += 1.3f;
			myBlock.transform.position = pos;
			
		}
	}


}
