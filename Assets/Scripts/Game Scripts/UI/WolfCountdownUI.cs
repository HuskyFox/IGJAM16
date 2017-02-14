using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* This script displays the countdown when a new wolf is created.
 * It is enabled by the NewWolfManager script.
 * The countdown value is set in the TimeManager script.*/
public class WolfCountdownUI : MonoBehaviour
{
	[HideInInspector] public float wolfCountdown;
	[HideInInspector] public bool currentlyPlaying;
	[SerializeField] private Text _wolfTimer;
	[SerializeField] private TimeManager _timeManager;
	[SerializeField] private GunAnimation _gun;
	private float _elapsedTime;
	private float _timeLeft;
	private bool _animPlaying;

	//The NewWolfManager script enables the GameObject this script is attached to every time a new wolf is created.
	void OnEnable()
	{
		//reset everything and start the countdown.
		currentlyPlaying = true;
		_animPlaying = false;
		_elapsedTime = 0f;
		_timeLeft = wolfCountdown;
		StartCoroutine (Countdown ());
	}

	IEnumerator Countdown()
	{
		while (Mathf.Floor (_elapsedTime) != wolfCountdown) 
		{
			DisplayTime ();

			//stop the countdown while the game is on Pause.
			//The bool currentlyPlaying is controlled by the GameStateManager.
			if (!currentlyPlaying)
				yield return StartCoroutine (WaitForUnpause ());

			yield return null;

			_elapsedTime += Time.deltaTime;		
		}

		yield return null;

		gameObject.SetActive (false);
	}

	IEnumerator WaitForUnpause()
	{
		//stop the gun animation and the countdown.
		_gun.PauseAnimToggle ();
		while (!currentlyPlaying)
		{
			yield return null;
		}
		_gun.PauseAnimToggle ();
	}

	void DisplayTime()
	{
		_timeLeft = wolfCountdown - Mathf.Floor (_elapsedTime);
		_wolfTimer.text = _timeLeft.ToString ("F0");

		//for the gun animation synchronisation.
		if(_timeLeft == 1f && !_animPlaying) 
		{
			_gun.TriggerGunShot ();
			_animPlaying = true;
		}
	}
}
