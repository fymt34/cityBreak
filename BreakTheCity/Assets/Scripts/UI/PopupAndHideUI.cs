using UnityEngine;

public class PopupAndHideUI : MonoBehaviour
{
	public GameObject popupUI;
	public GameObject hideUI;

	public void PopupAndHide()
	{
		popupUI.SetActive(true);
		hideUI.SetActive(false);
		SoundManager.Instance.PlaybuttonSE();
	}
}
