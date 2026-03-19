using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayScore : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private TextMeshProUGUI highScoreText;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		scoreText.text = ("Score:") + ScoreManager.Instance.Score.ToString();
		ScoreManager.Instance.ResetScore();
		highScoreText.text = ("High Score:") + ScoreManager.Instance.HighScore.ToString();
	}

	public void Pushbotten()
	{
		ScoreManager.Instance.ResetHighScore();
		SoundManager.Instance.PlaybuttonSE();
	}
}
