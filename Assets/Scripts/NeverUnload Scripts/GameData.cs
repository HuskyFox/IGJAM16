using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

//This script contains the important data that needs to survive between scenes (menu, game, duration, etc).
//Currently holds the list of registered players to instantiate in the game scene.
//Also the game duration chosen by the players.
public class GameData : MonoBehaviour 
{
	//The players are registered in a dictionary.
	//They have an int that is their player index (used by some scripts such as score), and a dedicated controller.
	public Dictionary <int, InputDevice> registeredPlayers = new Dictionary<int, InputDevice>();
	public float _gameDuration { get; private set;}

	void OnEnable()
	{
		ControllersRegistration.OnNewPlayerRegistered += AddPlayerToList;
		PlaytestRegistration.OnNewPlayerRegisteredForPlaytest += AddPlayerToList;
		StartReturnButtons.OnReturnToMenu += ClearPlayersList;
		GameTimeSetter.OnGameDurationModified += UpdateDuration;
	}

	//Creates a player with an index and a controller
	void AddPlayerToList(int playerIndex, InputDevice controller)
	{
		if (!registeredPlayers.ContainsKey (playerIndex))
			registeredPlayers.Add (playerIndex, controller);
	}

	void ClearPlayersList()
	{
		registeredPlayers.Clear ();
	}

	//holds the game duration chosen by the player until the GameManager get it from the GameScene.
	void UpdateDuration(int index)
	{
		switch (index)
		{
		case 0:
			_gameDuration = 120f;
			break;
		case 1:
			_gameDuration = 150f;
			break;
		case 2:
			_gameDuration = 180f;
			break;
		case 3:
			_gameDuration = 240f;
			break;
		case 4:
			_gameDuration = 300f;
			break;
		}
	}

	void OnDisable()
	{
		ControllersRegistration.OnNewPlayerRegistered -= AddPlayerToList;
		PlaytestRegistration.OnNewPlayerRegisteredForPlaytest -= AddPlayerToList;
		StartReturnButtons.OnReturnToMenu -= ClearPlayersList;
		GameTimeSetter.OnGameDurationModified -= UpdateDuration;
	}
}