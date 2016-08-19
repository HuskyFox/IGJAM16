using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour 
{
//	public AudioSource efxSource;

	public static SoundManager instance = null;

	[HideInInspector]public AudioSource[] sounds;
	bool slowMo;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	void Start()
	{
		sounds = GetComponents<AudioSource> ();
	}

//	void Update()
//	{
//		if (Time.timeScale == .1f)
//			slowMo = true;
//		else if (Time.timeScale == 1f)
//			slowMo = false;
//		
//		if (slowMo)
//		{
//			for (int i = 0; i < SoundManager.instance.sounds.Length - 1; i++) 
//			{
//				SoundManager.instance.sounds [i].pitch = .6f;
//			}
//		}
//		else if (!slowMo)
//		{
//			for (int i = 0; i < SoundManager.instance.sounds.Length - 1; i++) 
//			{
//				SoundManager.instance.sounds [i].pitch = 1f;
//			}
//		}
//	}

	public void PlaySheepBaa1()
	{
//		sounds [0].pitch = Random.Range (0.9f, 1.1f);
		sounds [0].Play ();
	}

	public void PlaySheepBaa2()
	{
//		sounds [1].pitch = Random.Range (0.9f, 1.1f);
		sounds [1].Play();
	}

	public void PlaySheepBaa3()
	{
//		sounds [2].pitch = Random.Range (0.9f, 1.1f);
		sounds [2].Play ();
	}

	public void PlaySheepBaa4()
	{
//		sounds [3].pitch = Random.Range (0.9f, 1.1f);
		sounds [3].Play();
	}

	public void PlayRandomSheepBaa()
	{
		int randomIndex = Random.Range (0, 4);
		switch (randomIndex)
		{
		case 0:
			PlaySheepBaa1 ();
			break;
		case 1:
			PlaySheepBaa2 ();
			break;
		case 2:
			PlaySheepBaa3 ();
			break;
		case 3:
			PlaySheepBaa4 ();
			break;
		}
	}

	public void PlayFarmerShout1()
	{
//		sounds [4].pitch = Random.Range (0.9f, 1.1f);
		sounds [4].PlayDelayed (0.5f);
	}

	public void PlayFarmerShout2()
	{
//		sounds [5].pitch = Random.Range (0.9f, 1.1f);
		sounds [5].PlayDelayed (0.5f);
	}

	public void PlayFarmerShout3()
	{
//		sounds [6].pitch = Random.Range (0.9f, 1.1f);
		sounds [6].PlayDelayed (0.5f);
	}

	public void PlayFarmerShout4()
	{
//		sounds [7].pitch = Random.Range (0.9f, 1.1f);
		sounds [7].PlayDelayed (0.5f);
	}

	public void PlayRandomFarmerShout()
	{
		int randomIndex = Random.Range (0, 4);
		switch (randomIndex)
		{
		case 0:
			PlayFarmerShout1 ();
			break;
		case 1:
			PlayFarmerShout2 ();
			break;
		case 2:
			PlayFarmerShout3 ();
			break;
		case 3:
			PlayFarmerShout4 ();
			break;
		}
	}

	public void PlayGunLoad()
	{
//		sounds [8].pitch = Random.Range (0.9f, 1.1f);
		sounds [8].Play ();
	}

	public void PlayWolfHowl()
	{
		sounds [9].Play ();
	}

	public void PlayWolfBiteFail()
	{
		sounds [10].Play ();
	}

	public void PlayWolfBiteSuccess()
	{
		sounds [11].Play ();
		sounds [17].PlayDelayed (2.5f);
	}

	public void PlayAmbianceField()
	{
		sounds [12].Play ();
	}

	public void StopAmbianceField()
	{
		sounds [12].Stop ();
	}

	public void PlaySheepReactionToHowl()
	{
		sounds[13].Play(2500);
	}
		
	public void PlayGameMusic()
	{
		sounds [14].PlayDelayed (1f);
	}

	public void StopGameMusic()
	{
		sounds [14].Stop ();
	}

	public void PauseGameMusic()
	{
		sounds [14].Pause ();
	}

	public void UnpauseGameMusic()
	{
		sounds [14].UnPause ();
	}

	public void PlayRestartMusic()
	{
		sounds [15].PlayDelayed (0.25f);
	}

	public void StopRestartMusic()
	{
		sounds [15].Stop ();
	}

	public void PlaySuccessSound()
	{
		sounds [16].Play();
	}

	public void PlayGunShot()
	{
		sounds [18].Play ();
	}
	/*public void PlayAmbianceRandomSheepSounds()
	{
		StartCoroutine(AmbianceRandomSheepSounds(Random.Range (0.5f, 3f)));
	}

	IEnumerator AmbianceRandomSheepSounds(float delay)
	{
		while (!timer.gameFinished)
		{
			PlayRandomSheepBaa ();
			yield return new WaitForSeconds (delay);
		}
		*/
//	}
}
