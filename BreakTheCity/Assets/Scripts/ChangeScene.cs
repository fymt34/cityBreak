using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	[SerializeField] private string sceneName;

	public void Change_button()
	{
		FadeOut.Instance.FadeOutStart();
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 1;
		SoundManager.Instance.PlaystartButtonSE();
		SceneManager.LoadScene(sceneName);
	}
}