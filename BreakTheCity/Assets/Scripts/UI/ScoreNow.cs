using TMPro;
using UnityEngine;

public class ScoreNow : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI scoreText;

	private void Start()
	{
		scoreText.enabled = true;
	}

	void Update()
    {
		scoreText.text = ("Score:") + ScoreManager.Instance.Score.ToString();
		if(Time.timeScale == 0)
		{
			scoreText.enabled = false;
		}
	}
}
