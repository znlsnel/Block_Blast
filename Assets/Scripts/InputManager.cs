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
		//bool click = obj.ReadValue<float>() > 0;
		//if (click)  
			Debug.Log("������");


	}
	void RelaseScreen(InputAction.CallbackContext obj) // Action�� ����� ��
	{
		//bool click = obj.ReadValue<float>() > 0;
		//if (click)  
		Debug.Log("������");


	}
	


	void MoveHandPos(InputAction.CallbackContext obj) // Action�� ����� ��
	{ 

	//	Debug.Log(obj.ReadValue<Vector2>());
	}


}
