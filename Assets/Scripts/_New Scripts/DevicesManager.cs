using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;

//this singleton is used to assign the devices to the players
public class DevicesManager : UnitySingleton <DevicesManager> 
{
	public int minPlayers = 1;
	const int maxPlayers = 4;

	[HideInInspector]public List<PlayerController> players = new List<PlayerController>( maxPlayers );
	[HideInInspector]public List<InputDevice> playerDevices = new List<InputDevice> (maxPlayers);

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
			print ("input check");
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

	//the function that finds the player that has the index of the device currently checked
	//and assigns this device to this player.
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
