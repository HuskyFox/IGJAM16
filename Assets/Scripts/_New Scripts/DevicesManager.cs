using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;

//this singleton is used to assign the devices to the players
public class DevicesManager : UnitySingleton <DevicesManager> 
{
	//TEMPORARY TEST FOR PLAYERSPAWN - DO THAT IN INPUTCHECK FOR GAMEISEADY
	public delegate void GameIsReady (List<PlayerController> registeredPlayers);
	public static event GameIsReady OnGameIsReady;


	public int minPlayers = 1;
	const int maxPlayers = 4;

	[HideInInspector]public List<PlayerController> players = new List<PlayerController>( maxPlayers );

	void Update()
	{
		//as long as there could be more players, the script continues to assign devices to players
		if(players.Count < maxPlayers )
		{
			var inputDevice = InputManager.ActiveDevice;

			if (JoinButtonWasPressedOnDevice( inputDevice ))
			{
				if (ThereIsNoPlayerUsingDevice( inputDevice ))
				{
					AssignDeviceToPlayer( inputDevice );
					print ("Number of players:" + players.Count);
				}
			}
			//print ("input check");
		}

		//TEMPORARY TEST FOR PLAYERSPAWN - DO THAT IN INPUTCHECK FOR GAMEISEADY
		if(InputManager.ActiveDevice.Action3.WasPressed)
		{
			print ("Game is ready!");
			if(OnGameIsReady != null)
				OnGameIsReady(players);
		}
	}

	bool JoinButtonWasPressedOnDevice( InputDevice inputDevice )
	{
		return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed /*|| inputDevice.LeftStick.WasPressed*/;
	}

	PlayerController FindPlayerUsingDevice( InputDevice inputDevice )
	{
		var playerCount = players.Count;
		for (var i = 0; i < playerCount; i++)
		{
			var player = players[i];
			if (player.Device == inputDevice)
			{
				return player;
			}
		}

		return null;
	}

	bool ThereIsNoPlayerUsingDevice( InputDevice inputDevice )
	{
		return FindPlayerUsingDevice( inputDevice ) == null;
	}

	//the function that finds the player that has the index of the device currently checked,
	//assigns this device to this player,
	//and adds a player to the list.
	PlayerController AssignDeviceToPlayer( InputDevice inputDevice )
	{
		if (players.Count < maxPlayers)
		{
			int nextPlayer = players.Count + 1;

			GameObject gameObject = GameObject.Find("Player_"+nextPlayer);
			PlayerController player = gameObject.GetComponent<PlayerController>();
			player.Device = inputDevice;
			players.Add( player );

			return player;
		}

		return null;
	}
}
