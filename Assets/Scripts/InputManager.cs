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

	void Start()
	{
		actionAsset.Enable();

		clickAction.action.performed += ClickScreen;
		clickAction.action.canceled += RelaseScreen; 
		handAction.action.performed += MoveHandPos;
	}


	void ClickScreen(InputAction.CallbackContext obj) // Action�� ����� ��
	{

		//Debug.Log("������");
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Raycast�� ������Ʈ ����
		RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

		if (hit.collider != null) // ������Ʈ�� ������ ���
		{
			Debug.Log("Clicked Object: " + hit.collider.gameObject.name);
		}

	}
	void RelaseScreen(InputAction.CallbackContext obj) // Action�� ����� ��
	{
		//bool click = obj.ReadValue<float>() > 0;
		//if (click)  
	//	Debug.Log("������");


	}
	


	void MoveHandPos(InputAction.CallbackContext obj) // Action�� ����� ��
	{ 
	//	Debug.Log(obj.ReadValue<Vector2>());
	}


}
