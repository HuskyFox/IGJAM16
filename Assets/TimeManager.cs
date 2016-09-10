using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : UnitySingleton <TimeManager>
{
	public delegate void GameIsStarted ();
	public static event GameIsStarted OnGameIsStarted;

	public delegate void GameIsOver ();
	public static event GameIsOver OnGameIsOver;

	public GameObject wolfCountdownUI;
	public Text wolfCountdownLabel;
	public float wolfCountdown = 5f;
	bool timeForANewWolf;
	float wolfSeconds;
	float wolfTime;

	public Text timerLabel;
	public bool currentlyTiming;
	public float gameTimeInSec; //In seconds
	float time;
	float mins;
	float seconds;

	void Start()
	{
		time = gameTimeInSec;
	}

	//sets the timer label
	void Update()
	{
		timerLabel.text = string.Format ("{0:0}:{1:00}", mins, seconds);

		if (currentlyTiming == true) 
		{
			time -= Time.deltaTime;
			mins = Mathf.Floor (time / 60);
			seconds = Mathf.Floor (time % 60); //Use the euclidean division for the seconds.

			if (time <= 0) 
			{
				if (OnGameIsOver != null)
					OnGameIsOver ();
				
				currentlyTiming = false;
				time = 0;
				mins = 0;
				seconds = 0;
			}
		}

		if (timeForANewWolf)
		{
			wolfCountdownLabel.text = wolfSeconds.ToString ();
			wolfTime -= Time.deltaTime;
			wolfSeconds = Mathf.Floor (wolfTime % 60);

			if (wolfTime <= 1) 
			{
				print ("End of Wolf Time");
				//SetNewWolf();
				timeForANewWolf = false;
				wolfCountdownUI.SetActive (false);
			}
		}
	}

	public void NewWolfCountdownUI()
	{
		timeForANewWolf = true;
		wolfTime = wolfCountdown;
		wolfCountdownUI.SetActive (true);
	}
}
