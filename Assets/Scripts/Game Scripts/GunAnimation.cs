using UnityEngine;
using System.Collections;

/* This script handles the animation and sound cues of the gun/farmer.
 * It is triggered by the NewWolfManager, and synced with the WolfCountdownUI.*/
public class GunAnimation : MonoBehaviour 
{
	[SerializeField] private AudioSource _farmerAudioSource;
	[SerializeField] private AudioSource _gunAudioSource;
	[SerializeField] private ParticleSystem _gunPoof;
	[SerializeField] private ParticleSystem _gunPoofLines;
	[SerializeField] private ParticleSystem _gunFire;
	private Animator _anim;


	void Awake()
	{
		_anim = GetComponent<Animator> ();
	}

	//Called by NewWolfManager script.
	public void StartAnim()
	{
		_farmerAudioSource.enabled = true;
		_gunAudioSource.enabled = true;
		_anim.SetTrigger ("GunPopOut");
	}

	//Called by WolfCountdownUI script, one second before the end of the countdown.
	public void TriggerGunShot()
	{
		_anim.SetTrigger ("GunShot");
	}

	//Triggered by an animation event.
	public void FarmerShout()
	{
		SoundManager.Instance.PlayFarmerSequence (_farmerAudioSource, "Shout");
	}

	//Triggered by an animation event.
	public void LoadGun()
	{
		SoundManager.Instance.PlayFarmerSequence (_gunAudioSource, "LoadGun");
	}

	//Triggered by an animation event.
	public void GunShot()
	{
		SoundManager.Instance.PlayFarmerSequence (_gunAudioSource, "GunShot");
		_gunPoof.Play ();
		_gunPoofLines.Play ();
		_gunFire.Play ();
	}

	//Called by WolfCountdownUI script.
	public void PauseAnimToggle()
	{
		_anim.enabled = !_anim.isActiveAndEnabled;
	}

	//Called by NewWolfManager script if the countdown was running when the game ended.
	public void HideForGameOver()
	{
		_anim.SetTrigger ("GameOver");
		_farmerAudioSource.enabled = false;
		_gunAudioSource.enabled = false;
	}
}
