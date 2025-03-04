using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionUI : MonoBehaviour
{

    public void HomeButton()
    {
			SceneManager.LoadScene("Home");
	}

	public void RetryButton()
    {
        if (DataManager.Instance.retryCnt++ < 2)
			SceneManager.LoadScene("Play");

        else
        {
			AdManager.Instance.rewardedAd.ShowAd(() =>
			{
				SceneManager.LoadScene("Play");
				DataManager.Instance.retryCnt = 0;
			});
		}
	}

}
