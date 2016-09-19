using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using InControl;

public class MenuManager : MonoBehaviour
{
	public void ActivateControllerRegistration()
	{
		FindObjectOfType<DevicesManager> ().lookingForPlayers = true;
	}

	public void ChangeMainMenu(GameObject defaultHighlightedButton)
	{
		EventSystem.current.SetSelectedGameObject ( defaultHighlightedButton );
	}

	public void RestartGame()
	{	
		FindObjectOfType<GameOverManager> ().enabled = false;
	}
}
