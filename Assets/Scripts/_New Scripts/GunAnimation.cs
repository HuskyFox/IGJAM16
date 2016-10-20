using UnityEngine;
using System.Collections;

public class GunAnimation : MonoBehaviour 
{
	public AudioSource farmerAudioSource;
	public AudioSource gunAudioSource;
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
		
	public void StartAnim(float countdown)
	{
		//anim.SetTrigger ("GunAnim");
		anim.SetTrigger ("GunPopOut");
		Invoke ("TriggerGunShot", countdown -1f);
	}

	void TriggerGunShot()
	{
		anim.SetTrigger ("GunShot");
	}

	public void FarmerShout()
	{
		SoundManager.Instance.PlayFarmerSequence (farmerAudioSource, "Shout");
	}

	public void LoadGun()
	{
		SoundManager.Instance.PlayFarmerSequence (gunAudioSource, "LoadGun");
	}

	public void GunShot()
	{
		SoundManager.Instance.PlayFarmerSequence (gunAudioSource, "GunShot");
		gunPoof.Play ();
		gunPoofLines.Play ();
		gunFire.Play ();
	}

	public void HideForGameOver()
	{
		anim.SetTrigger ("GameOver");
	}
}
