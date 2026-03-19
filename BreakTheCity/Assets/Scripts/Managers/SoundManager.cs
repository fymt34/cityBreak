using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
	AudioSource bgmSource;
	AudioSource seSource;

	[Header("BGM")]
	public AudioClip titleBGM;
	public AudioClip normalBGM;
	public AudioClip scoreBGM;

	[Header("SE")]
	public AudioClip explosion1SE;
	public AudioClip explosion2SE;
	public AudioClip explosion3SE;
	public AudioClip enemyAttack1SE;
	public AudioClip enemyAttack2SE;
	public AudioClip enemyAttack3SE;

	public AudioClip playerstepSE;
	public AudioClip playerJumpSE;
	public AudioClip playerLandingSE;

	public AudioClip getGunSE;
	public AudioClip beamSE;

	public AudioClip buttonSE;
	public AudioClip startButtonSE;

	public bool DontDestroyEnabled = true;

	void Start()
	{
		bgmSource = GetComponent<AudioSource>();
		seSource = GetComponent<AudioSource>();

		if(bgmSource == null)
		{
			Debug.LogError("bgmSourceがありません");
			return;
		}
		if(seSource == null)
		{
			Debug.LogError("seSourceがありません");
			return;
		}
		if(DontDestroyEnabled)
		{
			// Sceneを遷移してもオブジェクトが消えないようにする
			DontDestroyOnLoad(gameObject);
		}
		SceneManager.activeSceneChanged += OnActiveSceneChanged;
		PlaytitleBGM();
	}

	public void PlaytitleBGM()
	{
		bgmSource.clip = titleBGM;
		bgmSource.loop = true;
		bgmSource.Play();
	}

	public void PlaynormalBGM()
	{
		bgmSource.clip = normalBGM;
		bgmSource.loop = true;
		bgmSource.Play();
	}

	public void PlayscoreBGM()
	{
		bgmSource.clip = scoreBGM;
		bgmSource.loop = true;
		bgmSource.Play();
	}

	public void PlayexplosionSE()
	{
		for(int i = 0; i < 3; i++)
		{
			int randomIndex = Random.Range(0, 3);
			switch(randomIndex)
			{
				case 0:
					seSource.PlayOneShot(explosion1SE);
					break;
				case 1:
					seSource.PlayOneShot(explosion2SE);
					break;
				case 2:
					seSource.PlayOneShot(explosion3SE);
					break;
			}
		}
	}
	public void PlayenemyAttackSE()
	{
		int randomIndex = Random.Range(0, 3);
		switch(randomIndex)
		{
			case 0:
				seSource.PlayOneShot(enemyAttack1SE);
				break;
			case 1:
				seSource.PlayOneShot(enemyAttack2SE);
				break;
			case 2:
				seSource.PlayOneShot(enemyAttack3SE);
				break;
		}
	}

	public void PlaybeamSE()
	{
		seSource.PlayOneShot(beamSE);
	}

	public void PlaygetGunSE()
	{
		seSource.PlayOneShot(getGunSE);
	}

	public void PlaybuttonSE()
	{
		seSource.PlayOneShot(buttonSE);
	}

	public void PlaystartButtonSE()
	{
		seSource.PlayOneShot(startButtonSE);
	}

	public void StopBGM()
	{
		seSource.Stop();
	}


	public void PlayplayerstepSE()
	{
		seSource.PlayOneShot(playerstepSE);
	}

	public void PlayplayerJumpSE()
	{
		seSource.PlayOneShot(playerJumpSE);
	}

	public void PlayplayerLandingSE()
	{
		seSource.PlayOneShot(playerLandingSE);
	}

	private void OnActiveSceneChanged(Scene current, Scene next)
	{
		if(next.name == "Title")
		{
			PlaytitleBGM();
		}
		else if(next.name == "GameMain")
		{
			PlaynormalBGM();
		}
	}
}
