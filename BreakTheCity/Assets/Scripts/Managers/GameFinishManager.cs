using UnityEngine;

public class GameFinishManager : Singleton<GameFinishManager>
{
	[SerializeField] private GameObject playingUI;
	[SerializeField] private GameObject endUI;
	private bool isGameFinished = false;

	void Start()
	{
		playingUI.SetActive(true);
		endUI.SetActive(false);
	}

	public void FinishGame()
	{
		if(isGameFinished)
			return;
		isGameFinished = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		SubCamera.Instance.SwitchToSubCamera();
		SoundManager.Instance.PlayscoreBGM();

		Time.timeScale = 0;

		playingUI.SetActive(false);
		endUI.SetActive(true);
	}
}
