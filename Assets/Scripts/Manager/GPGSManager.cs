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

			Debug.Log("�α��� ����!");
			// �α��� ���� �� ó��
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
		if (CheckLoginStatus() == false)
			return;

		// �������� ǥ��
		PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
	}

	public void LoadMyScore()
	{
		// �������忡�� �� ���� ��������
			PlayGamesPlatform.Instance.LoadScores(
		leaderboardID, // �������� ID
			LeaderboardStart.PlayerCentered, // �÷��̾� �߽����� ���� �ҷ�����
			1, // ���� 1���� ������ �������� (�� ������ ��������)
			LeaderboardCollection.Public, // ���� ��������
			LeaderboardTimeSpan.AllTime, // ��ü �Ⱓ
			(LeaderboardScoreData data) =>
			{
				if (data.Status == ResponseStatus.Success)
				{
					// �� ���� ��������
					if (data.PlayerScore != null)
					{
						long myScore = data.PlayerScore.value;
						DataManager.Instance.bestScore = (int)myScore;
						Debug.Log("�� ����: " + myScore);
					}
					else
					{
						Debug.Log("�������忡 ������ �����ϴ�.");
					}
				}
				else
				{
					Debug.LogError("���� �ҷ����� ����: " + data.Status);
				}
			}
		);
	}
	bool CheckLoginStatus()
	{
		if (Social.localUser.authenticated)
		{
			Debug.Log("�α��� ����: �α��ε�");
			Debug.Log("����� ID: " + Social.localUser.id);
			Debug.Log("����� �̸�: " + Social.localUser.userName);
			return true;
		}
		else
		{
			Debug.Log("�α��� ����: �α��ε��� ����");
			// �α����� �� �Ǿ� �ִٸ� �α��� �õ�
			return false;
		}
		return true;
	}

}
