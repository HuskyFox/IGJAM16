using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Timers;

//This script contains the time related variables, to be able to change them more conveniently.
//See GameTime UI, InitialCountdownUI and WolfCountdownUI scripts.
public class TimeManager : MonoBehaviour
{
	[SerializeField] private float _gameTimeInSeconds = 150f;
	[SerializeField] private float _wolfCountdown = 3f;
	[SerializeField] private float _initialCountdown = 3f;
	[SerializeField] private InitialCountdownUI _countdownTimer;
	[SerializeField] private GameTimeUI _gameTimer;
	[SerializeField] private WolfCountdownUI _wolfTimer;

	void Awake()
	{
		_countdownTimer.countdownTime = _initialCountdown;
		_wolfTimer.wolfCountdown = _wolfCountdown;
	}

	//called by the GameManager, assign the game duration depending on what the players chose in the menu (Controller Registration).
	public void GameDuration(float newDuration)
	{
		_gameTimeInSeconds = newDuration;
		_gameTimer.gameTimeInSec = _gameTimeInSeconds;
	}
}
