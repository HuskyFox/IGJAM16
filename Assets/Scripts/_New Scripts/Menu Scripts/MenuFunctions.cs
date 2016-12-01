using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

// This script contains a few functions used in the main menu and in the pause menu.
public class MenuFunctions : MonoBehaviour 
{
	public delegate void RestartGamePressed ();
	public static event RestartGamePressed OnRestartGamePressed;

	public delegate void MainMenuPressed();
	public static event MainMenuPressed OnMainMenuPressed;

	//changes the selected gameobject when navigating through the menus.
	public void ChangeMainMenu(GameObject defaultHighlightedButton)
	{
		EventSystem.current.SetSelectedGameObject ( defaultHighlightedButton );
	}

	public void RestartGame()
	{
		if (OnRestartGamePressed != null)
			OnRestartGamePressed ();
	}

	public void MainMenu()
	{
		if (OnMainMenuPressed != null)
			OnMainMenuPressed ();
	}

	public void Quit()
    {
        Application.Quit();
    }
}
