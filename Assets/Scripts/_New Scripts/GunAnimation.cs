using UnityEngine;
using System.Collections;

public class GunAnimation : MonoBehaviour 
{
	AudioSource farmerAudioSource;
	ParticleSystem gunPoof;
	ParticleSystem gunPoofLines;
	ParticleSystem gunFire;

	void Awake()
	{
		farmerAudioSource = GetComponent<AudioSource> ();
		gunPoof = transform.Find ("Gun Poof").GetComponent<ParticleSystem> ();
		gunPoofLines = transform.Find ("Gun Poof/White Lines").GetComponent<ParticleSystem> ();
		gunFire = transform.Find ("Gun Poof/GunFire").GetComponent<ParticleSystem> ();
	}
		
	public void StartAnim()
	{
		GetComponent<Animator> ().SetTrigger ("GunAnim");
	}

	public void FarmerShout()
	{
		SoundManager.Instance.PlayFarmerSequence (farmerAudioSource, "Shout");
	}

	public void LoadGun()
	{
		SoundManager.Instance.PlayFarmerSequence (farmerAudioSource, "LoadGun");
	}

	public void GunShot()
	{
		SoundManager.Instance.PlayFarmerSequence (farmerAudioSource, "GunShot");
		gunPoof.Play ();
		gunPoofLines.Play ();
		gunFire.Play ();
	}
}
