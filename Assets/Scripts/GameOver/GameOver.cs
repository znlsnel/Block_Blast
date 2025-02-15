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
        DataManager db = DataManager.Instance;
        scoreText.text = db.score.ToString();

        if (db.score > db.bestScore)
        { 
            gameOverText.text = "BEST SCORE ! !";
            db.UpdateBestScore();
            bestScoreEffect.SetActive(true);
		}
		else
			gameOverText.text = "GAME OVER . .";

	}

}
