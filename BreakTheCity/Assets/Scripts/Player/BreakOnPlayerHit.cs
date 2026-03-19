using SimplestarGame;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnPlayerHit : MonoBehaviour
{
	[SerializeField] int score = 10;

	void Break(Vector3 point)
	{
		if(!TryGetComponent(out VoronoiFragmenter fragmenter))
			return;

		EffectManager.Instance.PlayEffect(point);

		RaycastHit hit = new RaycastHit();
		hit.point = point;

		fragmenter.Fragment(hit);

		Vector2 screenPos = Camera.main.WorldToScreenPoint(point);

		ScoreManager.Instance.AddScore(score);
		UItransform.Instance.PlayUItransform(screenPos,score);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(!collision.gameObject.CompareTag("Attack"))
			return;

		SoundManager.Instance.PlayexplosionSE();

		//if(!TryGetComponent(out VoronoiFragmenter fragmenter))
		//	return;

		ContactPoint contact = collision.contacts[0];

		//EffectManager.Instance.PlayEffect(contact.point);

		Vector2 screenPos = Camera.main.WorldToScreenPoint(contact.point);

		//if(gameObject.CompareTag("Building"))
		//{
		//	ScoreManager.Instance.AddScore(10);
		//	UItransform.Instance.PlayUItransform(screenPos);
		//}
		//else if(gameObject.CompareTag("BigBuilding"))
		//{
		//	ScoreManager.Instance.AddScore(20);
		//	UItransform.Instance.PlayUItransform(screenPos);
		//}

		RaycastHit hit = new RaycastHit();
		hit.point = contact.point;

		//fragmenter.Fragment(hit);

		Break(contact.point);
	}

	private void OnParticleCollision(GameObject other)
	{
		//if(!TryGetComponent(out VoronoiFragmenter fragmenter))
		//	return;

		// パーティクル衝突位置を取得
		ParticleSystem ps = other.GetComponent<ParticleSystem>();
		if(ps == null)
			return;

		List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
		int count = ps.GetCollisionEvents(gameObject, events);

		//EffectManager.Instance.PlayEffect(transform.position);

		if(count > 0)
		{
			//RaycastHit hit = new RaycastHit();
			//hit.point = events[0].intersection;

			//fragmenter.Fragment(hit);

			//if(ScoreManager.Instance)
			//{
			//	ScoreManager.Instance.AddScore(10);

			//	Vector2 screenPos = Camera.main.WorldToScreenPoint(hit.point);
			//	UItransform.Instance.PlayUItransform(screenPos);
			//}
			Break(events[0].intersection);
		}
	}
}