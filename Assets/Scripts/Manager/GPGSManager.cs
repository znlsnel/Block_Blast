using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GPGSManager : Singleton<GPGSManager>
{
	string leaderboardID = "CggI4JSc70EQAhAD";
	protected override void Awake()
	{
		base.Awake();
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
			LoadMyScore();
			// Continue with Play Games Services
		}
		else
		{

		}
	}

	public void AddScoreToLeaderboard(int score)
	{
		if (CheckLoginStatus() == false)
			return;
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
		if (CheckLoginStatus() == false)
			return;

		// 리더보드 표시
		PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
	}

	public void LoadMyScore()
	{
		// 리더보드에서 내 점수 가져오기
			PlayGamesPlatform.Instance.LoadScores(
		leaderboardID, // 리더보드 ID
			LeaderboardStart.PlayerCentered, // 플레이어 중심으로 점수 불러오기
			1, // 상위 1개의 점수만 가져오기 (내 점수만 가져오기)
			LeaderboardCollection.Public, // 공개 리더보드
			LeaderboardTimeSpan.AllTime, // 전체 기간
			(LeaderboardScoreData data) =>
			{
				if (data.Status == ResponseStatus.Success)
				{
					// 내 점수 가져오기
					if (data.PlayerScore != null)
					{
						long myScore = data.PlayerScore.value;
						DataManager.Instance.bestScore = (int)myScore;
						Debug.Log("내 점수: " + myScore);
					}
					else
					{
						Debug.Log("리더보드에 점수가 없습니다.");
					}
				}
				else
				{
					Debug.LogError("점수 불러오기 실패: " + data.Status);
				}
			}
		);
	}
	bool CheckLoginStatus()
	{
		if (Social.localUser.authenticated)
		{
			Debug.Log("로그인 상태: 로그인됨");
			Debug.Log("사용자 ID: " + Social.localUser.id);
			Debug.Log("사용자 이름: " + Social.localUser.userName);
			return true;
		}
		else
		{
			Debug.Log("로그인 상태: 로그인되지 않음");
			// 로그인이 안 되어 있다면 로그인 시도
			return false;
		}
		return true;
	}

}
