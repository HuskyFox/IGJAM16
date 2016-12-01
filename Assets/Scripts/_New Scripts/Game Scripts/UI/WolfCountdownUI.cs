using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WolfCountdownUI : MonoBehaviour
{
	[SerializeField] Text wolfTimer;
	[SerializeField] TimeManager timeManager;
	[HideInInspector] public float wolfCountdown;
	[SerializeField] GunAnimation gun;
	[HideInInspector] public bool currentlyPlaying;
	private float _elapsedTime;
	private float _timeLeft;
	private bool _gunTriggered = false;

	void OnEnable()
	{
		//wolfCountdown = timeManager.wolfCountdown;
		currentlyPlaying = true;
		_gunTriggered = false;
		_elapsedTime = 0f;
		_timeLeft = wolfCountdown;
		StartCoroutine (Countdown ());
	}

	IEnumerator Countdown()
	{
		while (Mathf.Floor (_elapsedTime) != wolfCountdown) 
		{
			DisplayTime ();
			if (!currentlyPlaying)
				yield return StartCoroutine (WaitForUnpause ());

			if (Mathf.Floor (_elapsedTime) == (wolfCountdown - 1f) && !_gunTriggered) 
			{
				gun.TriggerGunShot ();
				_gunTriggered = true;
			}

			yield return null;

			_elapsedTime += Time.deltaTime;		
		}

		yield return null;

		gameObject.SetActive (false);
	}

	IEnumerator WaitForUnpause()
	{
		gun.PauseAnimToggle ();
		while (!currentlyPlaying)
		{
			yield return null;
		}
		gun.PauseAnimToggle ();
	}

	void DisplayTime()
	{
		_timeLeft = wolfCountdown - Mathf.Floor (_elapsedTime);
		wolfTimer.text = _timeLeft.ToString ("F0");
	}
}
