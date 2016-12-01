using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimeUI : MonoBehaviour 
{
	[SerializeField] Text gameTimer;
	[HideInInspector] public float gameTimeInSec;
	public bool currentlyPlaying = false;

	private float _mins;
	private float _secs;

	private float _elapsedTime;
	private float _timeLeft;

	public delegate void GameIsOver ();
	public static event GameIsOver OnGameIsOver;

	void Start()
	{
		ResetTimer ();
	}

	public void ResetTimer()
	{
		StopAllCoroutines ();
		_elapsedTime = 0f;
		currentlyPlaying = false;
		DisplayTime ();
	}

	public void StartGameTimer()
	{
		currentlyPlaying = true;
		StartCoroutine (GameTimer ());
	}

	IEnumerator GameTimer()
	{
		while (_elapsedTime < gameTimeInSec)
		{
			DisplayTime ();

			if (!currentlyPlaying)
				yield return StartCoroutine (WaitForUnpause ());
			
			yield return null;

			_elapsedTime += Time.deltaTime;
		}

		if (OnGameIsOver != null)
			OnGameIsOver ();
		
		gameTimer.text = string.Format ("{0:0}:{1:00}", 0, 0);

		//ResetTimer ();

	}

	IEnumerator WaitForUnpause()
	{
		while (!currentlyPlaying)
		{
			yield return null;
		}
	}

	void DisplayTime()
	{
		_timeLeft = gameTimeInSec - Mathf.Floor (_elapsedTime);
		_mins = Mathf.Floor (_timeLeft / 60);
		_secs = Mathf.Floor (_timeLeft % 60);
		gameTimer.text = string.Format ("{0:0}:{1:00}", _mins, _secs);
		//print ("Time left : " + _timeLeft + ", mins:" + _mins+ ", secs: "+_secs);
	}
}
