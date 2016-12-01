using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InitialCountdownUI : MonoBehaviour 
{
	[SerializeField] Text initCountdown;
	//[SerializeField] TimeManager timeManager;
	[HideInInspector] public float countdownTime;
	//[HideInInspector] public bool currentlyPlaying;
	private float _elapsedTime;
	private float _timeLeft;

	public delegate void GameIsStarted ();
	public static event GameIsStarted OnGameIsStarted;

	void OnEnable()
	{
		//wolfCountdown = timeManager.wolfCountdown;
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
		initCountdown.text = _timeLeft.ToString ("F0");
	}
}
