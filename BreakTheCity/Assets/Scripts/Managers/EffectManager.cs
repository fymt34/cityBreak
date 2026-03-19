using UnityEngine;
using System.Collections;

public class EffectManager : Singleton<EffectManager>
{
	[SerializeField] private ParticleSystem particle;
	[SerializeField] private ParticleSystem particle2;

	public void PlayEffect(Vector3 aTransform)
	{
		//オブジェクトの生成
		var explosion = Instantiate(particle.gameObject);
		//戻り値からゲームオブジェクトの場所の定義
		explosion.transform.position = aTransform;
		//パーティクルコンポーネントを取得する
		var ps = explosion.GetComponent<ParticleSystem>();
		ps.Play();
		StartCoroutine(StopEffect(ps, explosion));
	}
	
	public void PlayEnemyEffect(Vector3 aTransform)
	{
		//オブジェクトの生成
		var explosion = Instantiate(particle2.gameObject);
		//戻り値からゲームオブジェクトの場所の定義
		explosion.transform.position = aTransform;
		//パーティクルコンポーネントを取得する
		var ps = explosion.GetComponent<ParticleSystem>();
		ps.Play();
		StartCoroutine(StopEffect(ps, explosion));
	}

	IEnumerator StopEffect(ParticleSystem ps,GameObject explosion)
	{
		yield return new WaitForSeconds(1.0f);
		ps.Stop();
		Destroy(explosion);
	}
}