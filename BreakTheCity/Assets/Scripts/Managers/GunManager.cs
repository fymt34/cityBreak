using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GunManager : Singleton<GunManager>
{
	[SerializeField] private GameObject gunObj;
	[SerializeField] private GameObject popupGun;
	[SerializeField] private GameObject shotGun;

	public bool haveGun = false;

	private bool isAttacking;
	private float timer;

	void Start()
	{
		popupGun.SetActive(false);
		shotGun.SetActive(false);
		haveGun = false;
		isAttacking = false;
		timer = 0.0f;
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(1) && haveGun)
		{
			isAttacking = true;
			Destroy(popupGun);
			popupGun = null;
			shotGun.SetActive(true);
		}
		if(isAttacking)
		{
			timer += Time.deltaTime;
			if(timer >= 3.0f)
			{
				Destroy(shotGun);
				timer = 0.0f;
				haveGun = false;
				isAttacking = false;
			}
		}
	}

	public void GetGun()
	{
		popupGun.SetActive(true);
		haveGun = true;
	}
}
