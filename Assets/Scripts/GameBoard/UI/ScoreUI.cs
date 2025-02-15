using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI scoreText;
    DataManager db;
	void Start()
    {
        db = DataManager.Instance;

        scoreText.text = "0";
		db.onUpdateScore.AddListener(() => scoreText.text = db.score.ToString());
    }
     
}
