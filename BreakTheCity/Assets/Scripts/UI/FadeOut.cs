using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : Singleton<FadeOut>
{
	[SerializeField] Image image;

	float alpha = 1f;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		image.gameObject.SetActive(false);
	}

	public void FadeOutStart()
	{
		image.gameObject.SetActive(true);
		alpha = 1f;
		StartCoroutine(FadeOutEffect());
	}

	IEnumerator FadeOutEffect()
	{
		while(alpha > 0f)
		{
			alpha -= Time.deltaTime;
			Color color = image.color;
			color.a = alpha;
			image.color = color;	
			yield return null;
		}
		image.gameObject.SetActive(false);
	}
}
