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

	[SerializeField] private ControllersRegistration _contReg;
	[SerializeField] private GameObject _start;
	[SerializeField] private GameObject _mainMenu;

	//Clear the players' list (GameData script) when the main menu is loaded from the game scene.
	void Awake()
	{
		if (OnReturnToMenu != null)
			OnReturnToMenu ();
	}

	void Update()
	{
		//if there is enough registered players and Start is pressed...
		if (_contReg.controllers.Count >= _contReg.minPlayers && InputManager.ActiveDevice.Command.WasPressed)
		{
			if (OnGameIsReady != null)
				OnGameIsReady ();
		}

		//if B is pressed to return to the menu...
		if(InputManager.ActiveDevice.Action2.WasPressed)
		{
			if (OnReturnToMenu != null)
				OnReturnToMenu ();
			_mainMenu.SetActive (true);
			EventSystem.current.SetSelectedGameObject (_start);
			gameObject.SetActive (false);
		}
	}
}
