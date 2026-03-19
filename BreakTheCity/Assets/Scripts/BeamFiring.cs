using UnityEngine;

public class BeamFiring : MonoBehaviour
{
	[SerializeField] private ParticleSystem particle;
	[SerializeField] private GameObject gun;
	private bool isAttacking;
	private float lapseTime;
	private float attackTime;

	private void Start()
	{
		isAttacking = false;
		lapseTime = 0.0f;
		attackTime = 0.0f;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(1) && GunManager.Instance.haveGun == true)
		{
			if (!isAttacking)
			{
				particle.Play();
				isAttacking = true;
				SoundManager.Instance.PlaybeamSE();
			}
		}

		if (isAttacking)
		{
			attackTime += Time.deltaTime;
			if (attackTime >= 3.0f)
			{
				particle.Stop();
				attackTime = 0.0f;
			}

			lapseTime += Time.deltaTime;
			if (lapseTime >= 3.0f)
			{
				isAttacking = false;
				lapseTime = 0.0f;
			}
		}
	}
}
