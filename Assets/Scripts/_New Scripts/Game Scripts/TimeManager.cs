﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Timers;

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
		_gameTimer.gameTimeInSec = _gameTimeInSeconds;
		_wolfTimer.wolfCountdown = _wolfCountdown;
	}
}
