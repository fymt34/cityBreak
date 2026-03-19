using UnityEngine;
using System.Collections;

public class UItransform : Singleton<UItransform>
{
	[SerializeField] private RectTransform targetUI;
	[SerializeField] private RectTransform moveTarget;
	[SerializeField] private float moveDuration = 0.5f;

	[SerializeField] private Sprite score10Sprite;
	[SerializeField] private Sprite score20Sprite;
	[SerializeField] private Sprite score30Sprite;

	private UnityEngine.UI.Image image;

	float alpha = 1f;

	private Vector2 startPos;

	void Start()
	{
		targetUI.gameObject.SetActive(false);
		image = targetUI.GetComponent<UnityEngine.UI.Image>();
	}

	public void PlayUItransform(Vector2 screenPosition, int score)
	{
		StopAllCoroutines();

		targetUI.gameObject.SetActive(true);

		switch(score)
		{
			case 10:
				image.sprite = score10Sprite;
				break;
			case 20:
				image.sprite = score20Sprite;
				break;
			case 30:
				image.sprite = score30Sprite;
				break;
		}

		Color color = image.color;
		color.a = alpha;
		image.color = color;

		// âÊñ ç¿ïWÇUIç¿ïWÇ…ïœä∑
		targetUI.position = screenPosition;

		startPos = targetUI.position;

		StartCoroutine(UItransformEffect());
	}

	IEnumerator UItransformEffect()
	{
		yield return new WaitForSeconds(0.5f);

		float time = 0f;
		Vector2 endPos = moveTarget.position;

		while(time < moveDuration)
		{
			time += Time.deltaTime;
			float t = time / moveDuration;

			targetUI.position = Vector2.Lerp(startPos, endPos, t);

			Color color = image.color;
			color.a = alpha - t;
			image.color = color;

			yield return null;
		}

		targetUI.position = endPos;
		targetUI.gameObject.SetActive(false);
	}
}
