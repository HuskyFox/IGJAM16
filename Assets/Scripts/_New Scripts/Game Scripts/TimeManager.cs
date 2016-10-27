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

	public bool isGameStarted;
	[SerializeField]
	float gameTimeInSec; //In seconds
	float elapsedTime;

	public float wolfCountdown = 5f; //in seconds
	public bool timeForANewWolf;
	float wolfTime;

	[SerializeField]
	TimeUI UI;

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
		UI.gameTimeLeft = gameTimeInSec - Mathf.Floor (elapsedTime);
		UI.wolfSeconds = wolfCountdown - Mathf.Floor (wolfTime);

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
			//UI.showWolfCountdown = true;

			if (Mathf.Floor (wolfTime) == wolfCountdown)
			{
				timeForANewWolf = false;
				wolfTime = 0f;

				if (!isGameStarted)
				if (OnGameIsStarted != null)
					OnGameIsStarted ();
			}
		}
	}
}
