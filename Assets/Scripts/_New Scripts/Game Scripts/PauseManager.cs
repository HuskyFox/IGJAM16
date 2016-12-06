using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// This script handles the pausing/unpausing of the game (enables/disables the UI and triggers the events).
public class PauseManager : MonoBehaviour 
{
	[HideInInspector] public bool canPause = false;		//disable the pause function (i.e. during the initial countdown, or a kill).
	[SerializeField] private GameObject _pauseUI;
	private bool _gamePaused = false;

	public delegate void GamePaused();
	public static event GamePaused OnGamePaused;

	public delegate void GameUnpaused();
	public static event GameUnpaused OnGameUnpaused;

	void Update ()
	{
		if (InputManager.ActiveDevice.CommandWasPressed && canPause) 
		{
			_gamePaused = !_gamePaused;		//players can unpause by pressing start again.

			if(_gamePaused) 
			{
				_pauseUI.SetActive (true);
				if (OnGamePaused != null)
					OnGamePaused ();
			}
			else
			{
				_pauseUI.SetActive (false);
				UnpauseGame ();
			}
		}
	}

	public void UnpauseGame()
	{
		_gamePaused = false;
		if (OnGameUnpaused != null)
			OnGameUnpaused ();
	}

	//Used by the Restart Button.
	public void SetGamePausedBool()
	{
		_gamePaused = false;
	}
}
