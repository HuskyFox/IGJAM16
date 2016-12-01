using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour 
{
	[SerializeField] GameObject pauseUI;

	public delegate void GamePaused();
	public static event GamePaused OnGamePaused;

	public delegate void GameUnpaused();
	public static event GameUnpaused OnGameUnpaused;

	public bool gamePaused = false;
	public bool canPause = false;

	void Update ()
	{
		if (InputManager.ActiveDevice.CommandWasPressed && canPause) 
		{
			gamePaused = !gamePaused;

			if(gamePaused) 
			{
				pauseUI.SetActive (true);
				if (OnGamePaused != null)
					OnGamePaused ();
			}
			else
			{
				pauseUI.SetActive (false);
				UnpauseGame ();
			}
		}
	}

	public void UnpauseGame()
	{
		gamePaused = false;
		if (OnGameUnpaused != null)
			OnGameUnpaused ();
	}

	public void SetGamePausedBool()
	{
		gamePaused = false;
	}
}
