using UnityEngine;

public class GameQuiter : MonoBehaviour
{
	public void QuitGame()
	{
		Debug.Log("ゲーム終了"); // エディタではここだけ動く

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false; // エディタ停止
#else
        Application.Quit(); // ビルド後
#endif
	}
}
