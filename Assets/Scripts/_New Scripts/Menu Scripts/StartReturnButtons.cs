using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using InControl;

//This script handles the button functions during controller registration : Return and Start.
public class StartReturnButtons : MonoBehaviour 
{
	public delegate void GameIsReady ();
	public static event GameIsReady OnGameIsReady;

	public delegate void ReturnToMenu ();
	public static event ReturnToMenu OnReturnToMenu;

	[SerializeField]
	ControllersRegistration contReg;
	[SerializeField]
	GameObject start;
	[SerializeField]
	GameObject mainMenu;

	void Update()
	{
		//if there is enough registered players and Start is pressed...
		if (contReg.controllers.Count >= contReg.minPlayers && InputManager.ActiveDevice.Command.WasPressed)
		{
			if (OnGameIsReady != null)
				OnGameIsReady ();
		}

		//if B is pressed to return to the menu...
		if(InputManager.ActiveDevice.Action2.WasPressed)
		{
			if (OnReturnToMenu != null)
				OnReturnToMenu ();
			mainMenu.SetActive (true);
			EventSystem.current.SetSelectedGameObject (start);
			gameObject.SetActive (false);
		}
	}
}
