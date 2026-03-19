using UnityEngine;
using System.Collections;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
	public int Score
	{
		get; private set;
	}
	public int HighScore
	{
		get; private set;
	}

	public bool DontDestroyEnabled = true;

	private void Start()
	{
		if(DontDestroyEnabled)
		{
			// Sceneを遷移してもオブジェクトが消えないようにする
			DontDestroyOnLoad(gameObject);
		}
		Score = 0;
		HighScore = PlayerPrefs.GetInt("HighScore", 0);
	}

	private void Update()
	{
		if(Score > HighScore)
		{
			HighScore = Score;
			PlayerPrefs.SetInt("HighScore", HighScore);
			PlayerPrefs.Save();
		}
	}

	public void AddScore(int score)
	{
		if(score == 10)
		{
			StartCoroutine("ScoreEffect");
		}else if(score == 20)
		{
			StartCoroutine("ScoreDoubleEffect");
		}else if (score == 30)
		{
			StartCoroutine("ScoreTripleEffect");
		}
	}

	IEnumerator ScoreEffect()
	{
		yield return new WaitForSeconds(1.0f);
		Score += 10;
	}

	IEnumerator ScoreDoubleEffect()
	{
		yield return new WaitForSeconds(1.0f);
		Score += 20;
	}

	IEnumerator ScoreTripleEffect()
	{
		yield return new WaitForSeconds(1.0f);
		Score += 30;
	}

	public void ResetScore()
	{
		Score = 0;
	}

	public void ResetHighScore()
	{
		HighScore = 0;
		PlayerPrefs.DeleteKey("HighScore");
	}
}