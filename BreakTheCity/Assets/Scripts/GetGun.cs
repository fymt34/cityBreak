using UnityEngine;

public class GetGun : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			GunManager.Instance.GetGun();
			SoundManager.Instance.PlaygetGunSE();
			Destroy(this.gameObject);
		}
	}
}
