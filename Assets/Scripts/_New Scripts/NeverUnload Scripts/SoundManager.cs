using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : UnitySingleton <SoundManager>
{
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
	private AudioSource npSheepAudioSource;

	private int currentBaa = 0;

	List <GameObject> npSheep;

	public override void Awake()
	{
		base.Awake ();
		AudioListener.pause = false;
		if (SceneManager.GetActiveScene ().name == "Main Menu")
			PlayMenuMusic ();
	}

	public void PauseAudioToggle()
	{
		AudioListener.pause = !AudioListener.pause;
	}

	//****** MUSIC ******

	public void PlayMenuMusic()
	{
		menu.TransitionTo (0.1f);
		musicPlayer.Stop ();
		musicPlayer.clip = musics [0];
		musicPlayer.Play ();
	}
		
	public void PlayGameMusic()
	{
		AudioListener.pause = false;
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

	public void HeartBeatOn()
	{
		CancelInvoke ();
		musicPlayer.Pause ();
		heartBeatPlayer.Play ();
		killHeartBeat.TransitionTo (0.1f);
	}

	public void HeartBeatOff()
	{
		music.TransitionTo (0.1f);
		heartBeatPlayer.Stop ();
		musicPlayer.UnPause ();
		PlayRandomSheepBaa ();
	}

	public void StopMusic()
	{
		musicPlayer.Stop ();
	}


	// ****** SFX ******

	public void PlayRandomSheepBaa(AudioSource box)
	{
		//AudioSource playerAudio = player.GetComponent<AudioSource> ();
		box.clip = sheepBaas [Random.Range (0, sheepBaas.Length)];
		box.Play ();
	}

	public void PlayRandomSheepBaa ()
	{
		InvokeRepeating ("RandomSheepBaa", Random.Range (1f, 3f), Random.Range (3f, 5f)); 
	}

	void RandomSheepBaa()
	{
		currentBaa = GetRandomBaa ();
		npSheep = FindObjectOfType<NPSheepSpawner> ().npSheepInGame;
		npSheepAudioSource = npSheep [Random.Range (0, npSheep.Count)].GetComponent<AudioSource>();
		npSheepAudioSource.clip = sheepBaas[currentBaa];
		npSheepAudioSource.Play();
	}

	int GetRandomBaa()
	{
		int randomBaa = 0;
		do {
			randomBaa = Random.Range (0, sheepBaas.Length);
		} while (randomBaa == currentBaa);
		return randomBaa;
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

	public void PlayWolfHowl(PlayerActions wolf)
	{
		killerAudioSource = wolf.GetComponent<AudioSource> ();
		killerAudioSource.clip = wolfSounds [3];
		killerAudioSource.Play ();
	}

	public void PlaySheepReactionToHowl(PlayerActions wolf)
	{
		AudioSource.PlayClipAtPoint (sheepReactionToHowl, wolf.transform.position);
	}

	public void PlaySuccessKillSound(AudioSource killerAudio, AudioSource victimAudio, float delay)
	{
		StartCoroutine (SuccessKillSound (killerAudio, victimAudio, delay));
	}

	IEnumerator SuccessKillSound (AudioSource killer, AudioSource victim, float delay)
	{
		killer.clip = wolfSounds [1];
		killer.Play ();
		//victim.clip = sheepBaas [Random.Range (0, sheepBaas.Length)];
		//victim.Play ();

		yield return new WaitForSeconds (delay);

		killer.clip = wolfSounds [0];
		killer.Play ();
		victim.clip = poof;
		victim.Play ();
	}

	public void PlayFailKillSound (AudioSource killerAudio)
	{
		killerAudio.clip = poof;
		killerAudio.Play ();
	}
}
