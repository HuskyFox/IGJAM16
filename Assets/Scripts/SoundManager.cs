using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour 
{
//	public AudioSource efxSource;

	public static SoundManager instance = null;
//	public AudioClip sheepBaa1;
//	public AudioClip sheepBaa2;
//	public AudioClip sheepBaa3;
//	public AudioClip sheepBaa4;

	[HideInInspector]public AudioSource[] sounds;
//	Timer timer;
//
//	bool isGameStarted =false;

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
//		if(!isGameStarted && SceneManager.GetActiveScene().name=="Demo Scene") {
//			isGameStarted = true;
//			timer = GameObject.Find ("WolfPlayer").GetComponent<Timer> ();
//		}
//	}

	public void PlaySheepBaa1()
	{
		sounds [0].pitch = Random.Range (0.9f, 1.1f);
		sounds [0].Play ();
	}

	public void PlaySheepBaa2()
	{
		sounds [1].pitch = Random.Range (0.9f, 1.1f);
		sounds [1].Play();
	}

	public void PlaySheepBaa3()
	{
		sounds [2].pitch = Random.Range (0.9f, 1.1f);
		sounds [2].Play ();
	}

	public void PlaySheepBaa4()
	{
		sounds [3].pitch = Random.Range (0.9f, 1.1f);
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
		sounds [4].pitch = Random.Range (0.9f, 1.1f);
		sounds [4].Play ();
	}

	public void PlayFarmerShout2()
	{
		sounds [5].pitch = Random.Range (0.9f, 1.1f);
		sounds [5].Play ();
	}

	public void PlayFarmerShout3()
	{
		sounds [6].pitch = Random.Range (0.9f, 1.1f);
		sounds [6].Play ();
	}

	public void PlayFarmerShout4()
	{
		sounds [7].pitch = Random.Range (0.9f, 1.1f);
		sounds [7].Play ();
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

	public void PlayGunShot()
	{
		sounds [8].pitch = Random.Range (0.9f, 1.1f);
		sounds [8].Play ();
	}

	public void PlayWolfHowl()
	{
		sounds [9].pitch = Random.Range (0.9f, 1.1f);
		sounds [9].Play ();
	}

	public void PlayWolfBiteFail()
	{
		sounds [10].pitch = Random.Range (0.9f, 1.1f);
		sounds [10].Play ();
	}

	public void PlayWolfBiteSuccess()
	{
		sounds [11].pitch = Random.Range (0.9f, 1.1f);
		sounds [11].Play ();
	}

	public void PlayAmbianceField()
	{
		sounds [12].pitch = Random.Range (0.9f, 1.1f);
		sounds [12].Play ();
	}

	public void StopAmbianceField()
	{
		sounds [12].Stop ();
	}

	public void PlaySheepReactionToHowl()
	{
		sounds[13].pitch = Random.Range (0.9f, 1.1f) ;
		sounds[13].Play();
	}

	public void PlayGameMusic1()
	{
		sounds[14].Play();
	}

	public void PlayGameMusic2()
	{
		sounds[15].Play();
	}

	public void PlayGameMusic3()
	{
		sounds[16].Play();
	}

	public void PlayGameMusic4()
	{
		sounds[17].Play();
	}

	public void PlayRandomGameMusic()
	{
		int randomIndex = Random.Range (0, 4);
		print (randomIndex);
		switch (randomIndex)
		{
		case 0:
			PlayGameMusic1 ();
			break;
		case 1:
			PlayGameMusic2 ();
			break;
		case 2:
			PlayGameMusic3 ();
			break;
		case 3:
			PlayGameMusic4 ();
			break;
		}
	}

	public void PlayRestartMusic()
	{
		sounds [18].Play (1100);
	}

	public void StopRestartMusic()
	{
		sounds [18].Stop ();
	}

	public void PlayGameMusic()
	{
		sounds [19].Play (2200);
	}

	public void StopGameMusic()
	{
		sounds [19].Stop ();
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
