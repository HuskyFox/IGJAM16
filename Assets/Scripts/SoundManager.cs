using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour 
{
	public AudioSource efxSource;

	public static SoundManager instance = null;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle (AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.Play ();
	}

	public void RandomizeSfx (params AudioClip[] clips)
	{
		int randomIndex = Random.Range (0, clips.Length);

		efxSource.clip = clips[randomIndex];
		efxSource.Play();
	}


}
