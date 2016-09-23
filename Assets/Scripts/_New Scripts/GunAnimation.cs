using UnityEngine;
using System.Collections;

public class GunAnimation : MonoBehaviour 
{
	AudioSource farmerAudioSource;
	Animator anim;
	ParticleSystem gunPoof;
	ParticleSystem gunPoofLines;
	ParticleSystem gunFire;

	void Start()
	{
		farmerAudioSource = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		gunPoof = transform.Find ("Gun Poof").GetComponent<ParticleSystem> ();
		gunPoofLines = transform.Find ("Gun Poof/White Lines").GetComponent<ParticleSystem> ();
		gunFire = transform.Find ("Gun Poof/GunFire").GetComponent<ParticleSystem> ();
	}
		
	public void StartAnim()
	{
		anim.SetTrigger ("GunAnim");
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

	public void HideForGameOver()
	{
		anim.SetTrigger ("GameOver");
	}
}
