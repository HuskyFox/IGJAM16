using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
	public delegate void GameIsStarted ();
	public static event GameIsStarted OnGameIsStarted;

	public delegate void GameIsOver ();
	public static event GameIsOver OnGameIsOver;

	public float timerDelay = 0f;

	public float gameTimeInSec; //In seconds
	UIManager timeUI;
	float time;
	float mins;
	float seconds;

	public float wolfCountdown = 5f; //in seconds
	public bool timeForANewWolf { get; set;}
	float wolfSeconds;
	float wolfTime;

	public bool isGameStarted { get; set; }

	void OnEnable()
	{
		timeUI = GameObject.Find ("Canvas").GetComponent<UIManager> ();
		isGameStarted = false;
		time = gameTimeInSec;
		timeForANewWolf = true;
		wolfTime = wolfCountdown;
	}

	//sets the timer label
	void Update()
	{
		timeUI.TimeRemaining (mins, seconds);
		mins = Mathf.Floor (time / 60);
		seconds = Mathf.Floor (time % 60); //Use the euclidean division for the seconds.

		wolfSeconds = Mathf.Floor (wolfTime % 60);

//		if (!isGameStarted)
//		{
//			//create a countdown before the game starts!
//		}

		if (isGameStarted)
		{
			time -= Time.deltaTime;

			if (time  < timerDelay) 
			{
				if (OnGameIsOver != null)
					OnGameIsOver ();

				time = 0;
				mins = 0;
				seconds = 0;
			}
		}

		if (timeForANewWolf)
		{
			wolfTime -= Time.deltaTime;
			timeUI.WolfCountDown (wolfSeconds, timeForANewWolf);

			if (wolfTime < timerDelay) 
			{
				timeForANewWolf = false;
				wolfTime = wolfCountdown;
				timeUI.WolfCountDown (wolfSeconds, !timeForANewWolf);

				if(!isGameStarted)
					if (OnGameIsStarted != null)
						OnGameIsStarted ();
			}
		}
	}
}
