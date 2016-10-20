using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour 
{
	//public GameObject pauseMenu;
	public GameObject UIManager;
	public GameObject resumeButton;

	public delegate void GamePaused();
	public static event GamePaused OnGamePaused;

	private bool gamePaused = false;
	private UIManager UI;
	//private Animation anim;

	void Awake()
	{
		UI = UIManager.GetComponent<UIManager> ();
		//anim = pauseMenu.GetComponent<Animation> ();
	}

	void Update ()
	{
		if (InputManager.ActiveDevice.CommandWasPressed && !gamePaused) 
		{
			UI.DisplayPauseMenu ();
			EventSystem.current.SetSelectedGameObject (resumeButton);
			gamePaused = true;
			if (OnGamePaused != null)
				OnGamePaused ();
		}
	}

	public void ResumeGame()
	{
		FindObjectOfType<GameStateManager> ().GameIsStarted ();
		UI.HidePauseMenu ();
		gamePaused = false;
	}

	public void RestartGame()
	{	
		FindObjectOfType<GameStateManager> ().GoToGameScene ();
		UI.HidePauseMenu ();
		gamePaused = false;
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene ("Main Menu");
		UI.HidePauseMenu ();
		gamePaused = false;
	}

	public void QuitGame()
	{
		gamePaused = false;
		UI.HidePauseMenu ();
		print ("Exiting game...");
		Application.Quit ();
	}
}
