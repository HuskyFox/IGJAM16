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

	private Text timerLabel;
	//public bool currentlyTiming;
	public float gameTimeInSec; //In seconds
	float time;
	float mins;
	float seconds;

	private GameObject wolfCountdownUI;
	private Text wolfCountdownLabel;
	public float wolfCountdown = 5f; //in seconds
	bool timeForANewWolf;
	float wolfSeconds;
	float wolfTime;

	public bool isGameStarted { get; set; }

	void OnEnable()
	{
		timerLabel = GameObject.Find ("Timer").GetComponent<Text> ();
		wolfCountdownUI = GameObject.Find ("Canvas").transform.Find ("WolfCountDown").gameObject;
		wolfCountdownLabel = wolfCountdownUI.transform.Find ("WolfTimer").GetComponent<Text> ();

		isGameStarted = false;
		wolfTime = wolfCountdown;
		NewWolfCountdownUI ();

		time = gameTimeInSec;
	}

	//sets the timer label
	void Update()
	{
		timerLabel.text = string.Format ("{0:0}:{1:00}", mins, seconds);
		mins = Mathf.Floor (time / 60);
		seconds = Mathf.Floor (time % 60); //Use the euclidean division for the seconds.

		wolfCountdownLabel.text = wolfSeconds.ToString ();
		wolfSeconds = Mathf.Floor (wolfTime % 60);

//		if (!isGameStarted)
//		{
//			//create a countdown before the game starts!
//			//(using the wolf countdown for now).
//
//			//wolfCountdownLabel.text = wolfSeconds.ToString ();
//			wolfTime -= Time.deltaTime;
////			wolfSeconds = Mathf.Floor (wolfTime % 60);
//
//			if (wolfTime < timerDelay) 
//			{
//				wolfCountdownUI.SetActive (false);
//				if (OnGameIsStarted != null)
//					OnGameIsStarted ();
//			}
//		}
		if (isGameStarted)
		{

				time -= Time.deltaTime;
			
				if (time  < timerDelay) 
				{
					if (OnGameIsOver != null)
						OnGameIsOver ();

					//currentlyTiming = false;
					time = 0;
					mins = 0;
					seconds = 0;
				}
		
		}

		if (timeForANewWolf)
		{
			//wolfCountdownLabel.text = wolfSeconds.ToString ();
			wolfTime -= Time.deltaTime;
			//wolfSeconds = Mathf.Floor (wolfTime % 60);

			if (wolfTime < timerDelay) 
			{
				timeForANewWolf = false;
				wolfCountdownUI.SetActive (false);
				if(!isGameStarted)
					if (OnGameIsStarted != null)
						OnGameIsStarted ();
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
