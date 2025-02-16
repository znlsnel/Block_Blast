using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [SerializeField] GameObject closeButton;
    [SerializeField] GameObject panel;
    void Awake()
    {
        closeButton.SetActive(false);
		panel.SetActive(false);

	}

    public void OpenUI()
    {
		closeButton.SetActive(true);
		panel.SetActive(true); 
	}

	public void CloseUI()
	{
		closeButton.SetActive(false);
		panel.SetActive(false);
	} 

}
