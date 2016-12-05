using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* This script handles the initial countdown before the game starts.
 * Its value is set in the TimeManager script.
 * It is enabled by the GameStateManager script.*/
public class InitialCountdownUI : MonoBehaviour 
{
	[HideInInspector] public float countdownTime;
	[SerializeField] private Text _initCountdown;
	//[HideInInspector] public bool currentlyPlaying;
	private float _elapsedTime;
	private float _timeLeft;

	public delegate void GameIsStarted ();
	public static event GameIsStarted OnGameIsStarted;

	void OnEnable()
	{
		_elapsedTime = 0f;
		_timeLeft = countdownTime;
		StartCoroutine (Countdown ());
	}

	IEnumerator Countdown()
	{
		while (_elapsedTime < countdownTime) 
		{
			DisplayTime ();
//			if (!currentlyPlaying)
//				yield return StartCoroutine (WaitForUnpause ());

			yield return null;

			_elapsedTime += Time.deltaTime;		
		}
			
		if (OnGameIsStarted != null)
			OnGameIsStarted ();
		
		gameObject.SetActive (false);
	}

//	IEnumerator WaitForUnpause()
//	{
//		while (!currentlyPlaying)
//		{
//			yield return null;
//		}
//	}

	void DisplayTime()
	{
		_timeLeft = countdownTime - Mathf.Floor (_elapsedTime);
		_initCountdown.text = _timeLeft.ToString ("F0");
	}
}
