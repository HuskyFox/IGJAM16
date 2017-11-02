using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/* This script contains all the sound (music & SFX) related functions.
 * They are called by different scripts.*/
public class SoundManager : UnitySingleton <SoundManager>
{
	//Snaps for the sound mixing.
	[SerializeField] private AudioMixerSnapshot _menuSnap;
	[SerializeField] private AudioMixerSnapshot _gameSnap;
	[SerializeField] private AudioMixerSnapshot _musicSnap;
	[SerializeField] private AudioMixerSnapshot _killSnap;

	//Audio clips
	[SerializeField] private AudioClip[] _wolfSounds;
	[SerializeField] private AudioClip[] _sheepBaas;
	[SerializeField] private AudioClip _sheepReactionToHowl;
	[SerializeField] private AudioClip _poof;
	[SerializeField] private AudioClip[] _farmerShouts;
	[SerializeField] private AudioClip[] _gunSounds;
	[SerializeField] private AudioClip[] _musics;
	[SerializeField] private AudioClip _heartBeat;

	//Audio Sources
	[SerializeField] private AudioSource _musicPlayer;
	[SerializeField] private AudioSource _heartBeatPlayer;
	private AudioSource _killerAudioSource;
	private AudioSource _npSheepAudioSource;

	private int _currentBaa = 0;		//for random sheep baas
	private List <GameObject> _npSheep;	//for random sheep baas

	void Awake()
	{
		//base.Awake ();	//singleton
		AudioListener.pause = false;	//make sur the Audio is on.

		#if (UNITY_EDITOR)
		//Play the menu music if the main menu scene is already loaded/
		if (SceneManager.GetActiveScene ().name == "Main Menu")
			PlayMusic("Menu");
		#endif
	}

	//****** MUSIC ******

	public void PlayMusic(string music)
	{
		AudioListener.pause = false;	//make sure audio is on.
		CancelInvoke ();				//Stop random sheep baas (useful in case of Restart or Return to main menu).
		_musicPlayer.Stop ();

		//Choose the right music.
		if(music == "Menu")
		{
			_menuSnap.TransitionTo (0.1f);
			_musicPlayer.clip = _musics [0];
		}
		if(music == "Game")
		{
			_gameSnap.TransitionTo (0.1f);
			_musicPlayer.clip = _musics [1];
		}
		if(music == "GameOver")
		{
			_musicPlayer.clip = _musics [2];
		}

		_musicPlayer.Play ();
	}

	//Called by the KillManager
	public void HeartBeatOn()
	{
		CancelInvoke ();				//stop sheep baas
		_musicPlayer.Pause ();
		_heartBeatPlayer.Play ();
		//_killSnap.TransitionTo (0.1f);
	}

	//Called by the KillManager
	public void HeartBeatOff()
	{
		//_musicSnap.TransitionTo (0.1f);
		_heartBeatPlayer.Stop ();
		_musicPlayer.UnPause ();
		PlayRandomSheepBaa ();			//resume sheep baas
	}

	// ****** SFX ******

	//Randomly play a random sheep baa during the game.
	public void PlayRandomSheepBaa ()
	{
		InvokeRepeating ("RandomSheepBaa", Random.Range (1f, 3f), Random.Range (3f, 5f)); 
	}

	void RandomSheepBaa()
	{
		_currentBaa = GetRandomBaa ();

		//Get the current list of NPSheep still in the game.
		_npSheep = FindObjectOfType<NPSheepSpawner> ().npSheepInGame;

		//Pick one randomly and get its audiosource.
		_npSheepAudioSource = _npSheep [Random.Range (0, _npSheep.Count)].GetComponent<AudioSource>();
		_npSheepAudioSource.clip = _sheepBaas[_currentBaa];
		_npSheepAudioSource.Play();
	}

	int GetRandomBaa()
	{
		int randomBaa = 0;
		do {
			randomBaa = Random.Range (0, _sheepBaas.Length);
		} while (randomBaa == _currentBaa);
		return randomBaa;
	}

	//Overload method for the controllers registration.
	public void PlayRandomSheepBaa(AudioSource box)
	{
		box.clip = _sheepBaas [Random.Range (0, _sheepBaas.Length)];
		box.Play ();
	}

	//Called by the GunAnimation script. Synced with the gun animation events.
	public void PlayFarmerSequence(AudioSource farmer, string clip)
	{
		if (clip == "Shout")
			farmer.clip = _farmerShouts [Random.Range (0, _farmerShouts.Length)];
		if (clip == "LoadGun")
			farmer.clip = _gunSounds [0];
		if (clip == "GunShot")
			farmer.clip = _gunSounds [1];
		
		farmer.Play ();
	}

	//Called by the HowlManager script.
	public void PlayWolfHowl(PlayerActions wolf)
	{
		_killerAudioSource = wolf.GetComponent<AudioSource> ();
		_killerAudioSource.clip = _wolfSounds [2];	//howl sound.
		_killerAudioSource.Play ();
	}

	//Called by the HowlManager script.
	public void PlaySheepReactionToHowl(PlayerActions wolf)
	{
		//Play the sheep scattering sound where the wolf was when he howled.
		AudioSource.PlayClipAtPoint (_sheepReactionToHowl, wolf.transform.position);
	}

	//Called by the KillFeedback script (player script).
	public void PlayKillSound (AudioSource source, string clip)
	{	
		if(clip == "Growl")
			source.clip = _wolfSounds [0];
		 if(clip == "Bite")
			source.clip = _wolfSounds [1];
		if(clip == "Poof")
			source.clip = _poof;
		
		source.Play ();
	}
}
