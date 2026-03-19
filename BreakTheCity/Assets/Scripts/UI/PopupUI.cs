using UnityEngine;

public class PopupUI : MonoBehaviour
{
    public GameObject popupPanel;

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        SoundManager.Instance.PlaybuttonSE();
	}

    public void HidePopup()
    {
        popupPanel.SetActive(false);
        SoundManager.Instance.PlaybuttonSE();
	}
}