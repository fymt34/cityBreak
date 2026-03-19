using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
	[SerializeField] private float CountTime = 30f; // クリアまでの時間
	[SerializeField] private TextMeshProUGUI timeText;        // カウントダウン用UI

	private float timer = 0f;
	private bool isCleared = false;

	void Start()
	{
		timeText.enabled = true;
	}

	void Update()
	{
		if (isCleared) return;

		timer += Time.deltaTime;

		if (timeText != null)
		{
			timeText.text = "Time: " + (CountTime - timer).ToString("F1");
		}

		if (timer >= CountTime)
		{
			ClearGame();
		}
	}

	void ClearGame()
	{
		GameFinishManager.Instance.FinishGame();
	}
}

