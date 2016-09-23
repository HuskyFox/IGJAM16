using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Timers;

public class TimeManager : MonoBehaviour
{
	public delegate void GameIsStarted ();
	public static event GameIsStarted OnGameIsStarted;

	public delegate void GameIsOver ();
	public static event GameIsOver OnGameIsOver;

	public bool isGameStarted { get; set; }
	public float gameTimeInSec; //In seconds
	float elapsedTime;

	public float wolfCountdown = 5f; //in seconds
	public bool timeForANewWolf { get; set;}
	float wolfTime;

	UIManager timeUI;

	void OnEnable()
	{
		timeUI = GameObject.Find ("Canvas").GetComponent<UIManager> ();
	}

	public void ResetTime()
	{
		isGameStarted = false;
		elapsedTime = 0f;
		timeForANewWolf = true;
		wolfTime = 0f;
	}

	//sets the timer label
	void Update()
	{	
		timeUI.gameTimeLeft = gameTimeInSec - Mathf.Floor (elapsedTime);
		timeUI.wolfSeconds = wolfCountdown - Mathf.Floor (wolfTime);

		if(isGameStarted)
		{
			elapsedTime += Time.deltaTime;

			if (Mathf.Floor (elapsedTime) == gameTimeInSec)
			{
				timeForANewWolf = false;
				if (OnGameIsOver != null)
					OnGameIsOver ();
			}
		}

		if (timeForANewWolf) 
		{
			wolfTime += Time.deltaTime;
			timeUI.showWolfCountdown = true;

			if (Mathf.Floor (wolfTime) == wolfCountdown)
			{
				timeForANewWolf = false;
				wolfTime = 0f;

				if (!isGameStarted)
				if (OnGameIsStarted != null)
					OnGameIsStarted ();
			}
		}
		else if (!timeForANewWolf)
			timeUI.showWolfCountdown = false;
	}
}
