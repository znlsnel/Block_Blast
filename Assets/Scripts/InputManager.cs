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

	Block block = null;
	Vector3 handPos;
	void Start()
	{
		actionAsset.Enable();

		clickAction.action.performed += ClickScreen;
		clickAction.action.canceled += RelaseScreen; 
		handAction.action.performed += MoveHandPos;
	}


	void ClickScreen(InputAction.CallbackContext obj) // Action�� ����� ��
	{

		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

		if (hit.collider != null && hit.collider.GetComponent<Block>() != null) // ������Ʈ�� ������ ���
		{
			Debug.Log("Clicked Object: " + hit.collider.gameObject.name);
			block = hit.collider.GetComponent<Block>();
			block.OnDragMode();
		}

	}
	void RelaseScreen(InputAction.CallbackContext obj) // Action�� ����� ��
	{
		
		if (block != null && Board.instance.PutBlock(block) == false)
			block.OnPlayerDeck();
		 
		block = null;

	}
	


	void MoveHandPos(InputAction.CallbackContext obj) // Action�� ����� ��
	{
		handPos = Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>());
		if (block != null)
		{
			Vector3 pos = handPos;
			pos.z = 0;
			block.transform.position = pos;
		}
	}


}
