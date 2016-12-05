using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* This script handles the game timer.
 * The game time value is set in the TimeManager script.*/
public class GameTimeUI : MonoBehaviour 
{
	[HideInInspector] public float gameTimeInSec;
	[HideInInspector] public bool currentlyPlaying = false;
	[SerializeField] private Text _gameTimer;
	[SerializeField] private Animation _anim;	//pulsation anim for the last seconds.
	private float _mins;
	private float _secs;
	private float _elapsedTime;
	private float _timeLeft;
	private bool _animPlaying;

	public delegate void GameIsOver ();
	public static event GameIsOver OnGameIsOver;

	void Start()
	{
		ResetTimer ();
	}

	//Reset everything and display the game time.
	public void ResetTimer()
	{
		StopAllCoroutines ();
		_elapsedTime = 0f;
		currentlyPlaying = false;
		_animPlaying = false;
		DisplayTime ();
	}

	//Called by the GameStateManager
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

			//stop the countdown while the game is on Pause.
			//The bool currentlyPlaying is controlled by the GameStateManager.
			if (!currentlyPlaying)
				yield return StartCoroutine (WaitForUnpause ());
			
			yield return null;

			_elapsedTime += Time.deltaTime;
		}

		if (OnGameIsOver != null)
			OnGameIsOver ();
		
		_gameTimer.text = string.Format ("{0:0}:{1:00}", 0, 0);
		_anim.Stop ();
	}
		
	IEnumerator WaitForUnpause()
	{	
		//pause the timer animation if playing.
		_anim.enabled = false;
		while (!currentlyPlaying)
		{
			yield return null;
		}
		_anim.enabled = true;
	}

	void DisplayTime()
	{
		_timeLeft = gameTimeInSec - Mathf.Floor (_elapsedTime);
		_mins = Mathf.Floor (_timeLeft / 60);
		_secs = Mathf.Floor (_timeLeft % 60);
		_gameTimer.text = string.Format ("{0:0}:{1:00}", _mins, _secs);

		//Start the timer animation for the last five seconds.
		if (_timeLeft == 5f && !_animPlaying) 
		{
			_anim.Play ();
			_animPlaying = true;
		}	
	}
}
