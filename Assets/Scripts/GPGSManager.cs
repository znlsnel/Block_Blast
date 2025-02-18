using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;


public class GPGSManager : Singleton<GPGSManager>
{
	string leaderboardID = "CggI4JSc70EQAhAD";
	void Start()
	{
		
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
	}

	void ProcessAuthentication(SignInStatus status)
	{
		if (status == SignInStatus.Success)
		{
			
			Debug.Log("�α��� ����!");
			// �α��� ���� �� ó��
			string userID = Social.localUser.id;
			string userName = Social.localUser.userName;
			Debug.Log("User ID: " + userID);
			Debug.Log("User Name: " + userName);
			// Continue with Play Games Services
		}
		else
		{

		}
	}

	public void AddScoreToLeaderboard(int score)
	{
		// �������忡 ���� �߰�
		Social.ReportScore(score, leaderboardID, (bool success) =>
		{
			if (success)
			{
				Debug.Log("���� �߰� ����!");
			}
			else
			{
				Debug.LogError("���� �߰� ����");
			}
		});
	}
	 
	public void ShowLeaderboard()
	{
		// �������� ǥ��
		PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
	}

}
