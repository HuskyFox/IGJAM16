using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SoundManager : UnitySingleton <SoundManager>
{
	//public AudioSource[] sounds;
	public AudioMixerSnapshot menu;
	public AudioMixerSnapshot game;
	public AudioMixerSnapshot music;
	public AudioMixerSnapshot killHeartBeat;

	public AudioClip[] sheepBaas;
	public AudioClip sheepReactionToHowl;
	public AudioClip poof;
	public AudioClip[] farmerShouts;
	public AudioClip[] wolfSounds;
	public AudioClip[] gunSounds;
	public AudioClip[] musics;
	public AudioClip heartBeat;
	public AudioSource musicPlayer;
	public AudioSource heartBeatPlayer;

	private AudioSource killerAudioSource;
	private AudioSource victimAudioSource;
	//private AudioSource farmerAudioSource;

	public override void Awake()
	{
		base.Awake ();
		FindObjectOfType<GameStateManager> ().dontDestroy.Add (this.gameObject);
		PlayMenuMusic ();
	}

	void Start()
	{
		//sounds = GetComponents<AudioSource> ();
	}

	public void PlayRandomSheepBaa ()
	{
		InvokeRepeating ("RandomSheepBaa", Random.Range (1f, 3f), Random.Range (1f, 3f)); 
	}

//	public void StopRandomSheepBaa()
//	{
//		CancelInvoke ("RandomSheepBaa");
//	}

	void RandomSheepBaa()
	{
		print ("Baaaa");
		heartBeatPlayer.clip = sheepBaas[Random.Range (0, sheepBaas.Length)];
		heartBeatPlayer.Play ();
		//AudioSource npSheep = npSheepList [Random.Range (0, npSheepList.Count)];
		//npSheep.clip = sheepBaas[Random.Range (0, sheepBaas.Length)];
		//npSheep.Play();
	}

	public void PlayFarmerSequence(AudioSource farmer, string clip)
	{
		if (clip == "Shout")
			farmer.clip = farmerShouts [Random.Range (0, farmerShouts.Length)];
		if (clip == "LoadGun")
			farmer.clip = gunSounds [0];
		if (clip == "GunShot")
			farmer.clip = gunSounds [1];
		
		farmer.Play ();
	}

	public void PlayWolfHowl(PlayerController wolf)
	{
		killerAudioSource = wolf.GetComponent<AudioSource> ();
		killerAudioSource.clip = wolfSounds [3];
		killerAudioSource.Play ();
	}

	public void PlaySheepReactionToHowl(PlayerController wolf)
	{
		AudioSource.PlayClipAtPoint (sheepReactionToHowl, wolf.transform.position);
	}

	public void PlaySuccessKillSound(PlayerController killer, PlayerController victim, float delay)
	{
		killerAudioSource = killer.GetComponent<AudioSource> ();
		victimAudioSource = victim.GetComponent<AudioSource> ();

		StartCoroutine (SuccessKillSound (killerAudioSource, victimAudioSource, delay));
	}

	IEnumerator SuccessKillSound (AudioSource killer, AudioSource victim, float delay)
	{
		killer.clip = wolfSounds [1];
		killer.Play ();
		victim.clip = sheepBaas [Random.Range (0, sheepBaas.Length)];
		victim.Play ();

		yield return new WaitForSeconds (delay);

		killer.clip = wolfSounds [0];
		killer.Play ();
		victim.clip = poof;
		victim.Play ();
	}

	public void PlayFailKillSound (PlayerController killer)
	{
		killerAudioSource = killer.GetComponent<AudioSource> ();
		killerAudioSource.clip = poof;
		killerAudioSource.Play ();
	}

	public void PlayMenuMusic()
	{
		menu.TransitionTo (0.1f);
		musicPlayer.Stop ();
		musicPlayer.clip = musics [0];
		musicPlayer.Play ();
	}

	public void PlayGameMusic()
	{
		game.TransitionTo (0.1f);
		musicPlayer.Stop ();
		musicPlayer.clip = musics [1];
		musicPlayer.Play ();
	}

	public void PlayGameOverMusic()
	{
		musicPlayer.Stop ();
		musicPlayer.clip = musics [2];
		musicPlayer.Play ();
	}

	public void PauseMusic()
	{
		//musicPlayer.Pause ();
		heartBeatPlayer.Play ();
		killHeartBeat.TransitionTo (0.1f);

	}

	public void UnPauseMusic()
	{
		music.TransitionTo (0.1f);
		heartBeatPlayer.Stop ();
		//musicPlayer.UnPause ();
	}

//	public void PlaySheepBaa1()
//	{
////		sounds [0].pitch = Random.Range (0.9f, 1.1f);
//		sounds [0].Play ();
//	}
//
//	public void PlaySheepBaa2()
//	{
////		sounds [1].pitch = Random.Range (0.9f, 1.1f);
//		sounds [1].Play();
//	}
//
//	public void PlaySheepBaa3()
//	{
////		sounds [2].pitch = Random.Range (0.9f, 1.1f);
//		sounds [2].Play ();
//	}
//
//	public void PlaySheepBaa4()
//	{
////		sounds [3].pitch = Random.Range (0.9f, 1.1f);
//		sounds [3].Play();
//	}

	public void PlayRandomSheepBaa(PlayerController player)
	{
		//int randomIndex = Random.Range (0, sheepBaas.Length);
		AudioSource playerAudio = player.GetComponent<AudioSource> ();
		playerAudio.clip = sheepBaas [Random.Range (0, sheepBaas.Length)];
		playerAudio.Play ();

		//		switch (randomIndex)
//		{
//		case 0:
//			PlaySheepBaa1 ();
//			break;
//		case 1:
//			PlaySheepBaa2 ();
//			break;
//		case 2:
//			PlaySheepBaa3 ();
//			break;
//		case 3:
//			PlaySheepBaa4 ();
//			break;
//		}
	}

//	public void PlayFarmerShout1()
//	{
////		sounds [4].pitch = Random.Range (0.9f, 1.1f);
//		sounds [4].PlayDelayed (0.5f);
//	}
//
//	public void PlayFarmerShout2()
//	{
////		sounds [5].pitch = Random.Range (0.9f, 1.1f);
//		sounds [5].PlayDelayed (0.5f);
//	}
//
//	public void PlayFarmerShout3()
//	{
////		sounds [6].pitch = Random.Range (0.9f, 1.1f);
//		sounds [6].PlayDelayed (0.5f);
//	}
//
//	public void PlayFarmerShout4()
//	{
////		sounds [7].pitch = Random.Range (0.9f, 1.1f);
//		sounds [7].PlayDelayed (0.5f);
//	}

	public void PlayRandomFarmerShout()
	{
		//int randomIndex = Random.Range (0, 4);
//		switch (randomIndex)
//		{
//		case 0:
//			PlayFarmerShout1 ();
//			break;
//		case 1:
//			PlayFarmerShout2 ();
//			break;
//		case 2:
//			PlayFarmerShout3 ();
//			break;
//		case 3:
//			PlayFarmerShout4 ();
//			break;
//		}
	}
//
//	public void PlayGunLoad()
//	{
////		sounds [8].pitch = Random.Range (0.9f, 1.1f);
//		sounds [8].Play ();
//	}
//
//	public void PlayWolfHowl()
//	{
//		sounds [9].Play ();
//	}
//
//	public void PlayWolfBiteFail()
//	{
//		sounds [10].Play ();
//	}
//
//	public void PlayWolfBiteSuccess()
//	{
//		sounds [11].Play ();
//		sounds [17].PlayDelayed (2.5f);
//	}
//
//	public void PlayAmbianceField()
//	{
//		sounds [12].Play ();
//	}
//
//	public void StopAmbianceField()
//	{
//		sounds [12].Stop ();
//	}
//
//	public void PlaySheepReactionToHowl()
//	{
//		sounds[13].Play(2500);
//	}
//		
//	public void PlayGameMusic()
//	{
//		sounds [14].PlayDelayed (1f);
//	}
//
//	public void StopGameMusic()
//	{
//		sounds [14].Stop ();
//	}
//
//	public void PauseGameMusic()
//	{
//		sounds [14].Pause ();
//	}
//
//	public void UnpauseGameMusic()
//	{
//		sounds [14].UnPause ();
//	}
//
//	public void PlayRestartMusic()
//	{
//		sounds [15].PlayDelayed (0.25f);
//	}
//
//	public void StopRestartMusic()
//	{
//		sounds [15].Stop ();
//	}
//
//	public void PlaySuccessSound()
//	{
//		sounds [16].Play();
//	}
//
//	public void PlayGunShot()
//	{
//		sounds [18].Play ();
//	}
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
