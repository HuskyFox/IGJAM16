using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

//This script contains the important data that needs to survive between scenes (menu, game, etc).
//Currently holds the list of registered players to instantiate in the game scene.
public class GameData : MonoBehaviour 
{
	//The players are registered in a dictionary.
	//They have an int that is their player index (used by some scripts such as score), and a dedicated controller.
	public Dictionary <int, InputDevice> registeredPlayers = new Dictionary<int, InputDevice>();

	void OnEnable()
	{
		ControllersRegistration.OnNewPlayerRegistered += AddPlayerToList;
		PlaytestRegistration.OnNewPlayerRegisteredForPlaytest += AddPlayerToList;
		StartReturnButtons.OnReturnToMenu += ClearPlayersList;
	}

	void AddPlayerToList(int playerIndex, InputDevice controller)
	{
		if (!registeredPlayers.ContainsKey (playerIndex))
			registeredPlayers.Add (playerIndex, controller);
	}

	void ClearPlayersList()
	{
		registeredPlayers.Clear ();
	}

	void OnDisable()
	{
		ControllersRegistration.OnNewPlayerRegistered -= AddPlayerToList;
		PlaytestRegistration.OnNewPlayerRegisteredForPlaytest -= AddPlayerToList;
		StartReturnButtons.OnReturnToMenu -= ClearPlayersList;
	}
}

//public class Player
//{
//	public int index;
//	public InputDevice controller;
//	public PlayerController playerCont;	//rename that ?
//	public int scoreKeeper;
//	public bool movementEnabled;
//
//	List<Player> playersInGame = new List<Player> ();
//
//	public Player (int index, InputDevice controller)
//	{
//		Player newPlayer = new Player();
//		newPlayer.controller = InputDevice;
//		newPlayer.index = index;
//		newPlayer.playerCont = new PlayerController();
//
//	}
//}