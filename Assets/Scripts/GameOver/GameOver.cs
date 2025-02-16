using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Text gameOverText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject bestScoreEffect;

	private void Awake()
	{
        bestScoreEffect.SetActive(false);
	}
	void Start()
    { 
        

        StartCoroutine(UpdateScore());
	}
    
    void AE_OpenUI()
    {
		DataManager db = DataManager.Instance;
		if (db.score > db.bestScore)
		{
			gameOverText.text = "BEST SCORE ! !"; 
			db.UpdateBestScore();
			bestScoreEffect.SetActive(true); 
		}
		else
			gameOverText.text = "GAME OVER . .";
	}

    IEnumerator UpdateScore()
    {
        DataManager db = DataManager.Instance;
        float time = 1.0f ;

        float score = 0;
        int target = db.score;

        float d = db.score / time;

        while (time > 0)
        {
            time -= Time.deltaTime;
            score += d * Time.deltaTime;
			scoreText.text = ((int)score).ToString();
            yield return null;
		}
		scoreText.text = target.ToString();
	}

}
