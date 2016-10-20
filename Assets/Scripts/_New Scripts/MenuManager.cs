using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using InControl;

public class MenuManager : MonoBehaviour
{
//	public delegate void GamePaused();
//	public static event GamePaused OnGamePaused;
//
//	private bool gamePaused = false;
//
//	void Update ()
//	{
//		if (InputManager.ActiveDevice.CommandWasPressed && !gamePaused) 
//		{
//			gamePaused = true;
//			if (OnGamePaused != null)
//				OnGamePaused ();
//		}
//	}

	public void ActivateControllerRegistration()
	{
		FindObjectOfType<DevicesManager> ().lookingForPlayers = true;
	}

	public void ChangeMainMenu(GameObject defaultHighlightedButton)
	{
		EventSystem.current.SetSelectedGameObject ( defaultHighlightedButton );
	}

//	public void ResumeGame()
//	{
//		FindObjectOfType<GameStateManager> ().GameIsStarted ();
//		gamePaused = false;
//	}

	public void RestartGame()
	{	
		FindObjectOfType<GameStateManager> ().GoToGameScene ();
		FindObjectOfType<UIManager> ().InitGameUI ();
	}

//	public void GoToMainMenu()
//	{
//		SceneManager.LoadScene ("Main Menu");
//		gamePaused = false;
//	}
//
//	public void QuitGame()
//	{
//		gamePaused = false;
//		print ("Exiting game...");
//		Application.Quit ();
//	}
}
