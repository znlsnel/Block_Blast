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
			
			Debug.Log("로그인 성공!");
			// 로그인 성공 후 처리
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
		// 리더보드에 점수 추가
		Social.ReportScore(score, leaderboardID, (bool success) =>
		{
			if (success)
			{
				Debug.Log("점수 추가 성공!");
			}
			else
			{
				Debug.LogError("점수 추가 실패");
			}
		});
	}
	 
	public void ShowLeaderboard()
	{
		// 리더보드 표시
		PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
	}

}
