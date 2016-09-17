using UnityEngine;
using System.Collections;
using InControl;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//this singleton is used to assign the devices to the players
public class DevicesManager : UnitySingleton <DevicesManager>
{
	public delegate void GameIsReady ();
	public static event GameIsReady OnGameIsReady;

	public GameObject playerPrefab;

	public bool playtesting;

	public bool lookingForPlayers { get; set;}
	public int minPlayers = 1;
	const int maxPlayers = 4;

	[HideInInspector]public List<PlayerController> players = new List<PlayerController>( maxPlayers );

	void Update()
	{
		if (playtesting)
		{
				//as long as there could be more players, the script continues to assign devices to players
				if (players.Count < maxPlayers) 
				{
					var inputDevice = InputManager.ActiveDevice;

					if (JoinButtonWasPressedOnDevice (inputDevice))
					{
						print ("trying to join..");
						if (ThereIsNoPlayerUsingDevice (inputDevice)) 
						{
							AssignDeviceToPlayer (inputDevice);
						}
					}
					//				print ("input check");
				}

				if (InputManager.ActiveDevice.Action3.WasPressed) 
				{
					//GameStateManager.Instance.playersInGame = players;
					print ("Number of players:" + players.Count);
					print ("Game is ready!");
					if (OnGameIsReady != null)
						OnGameIsReady ();

				//SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
				}
		}
		else if (!playtesting && lookingForPlayers)
		{
			if (players.Count < maxPlayers) 
			{
				var inputDevice = InputManager.ActiveDevice;

				if (JoinButtonWasPressedOnDevice (inputDevice))
				{
					if (ThereIsNoPlayerUsingDevice (inputDevice)) 
					{
						AssignDeviceToPlayer (inputDevice);
					}
				}
				//				print ("input check");
			}

			if(InputManager.ActiveDevice.Action2.WasPressed)
			{
				GameObject.Find("Canvas").transform.Find ("MainMenu").gameObject.SetActive (true);
				GameObject.Find ("ControllerRegistration").gameObject.SetActive (false);
				players.Clear ();
			}

			if (InputManager.ActiveDevice.Command.WasPressed) 
			{
				//GameStateManager.Instance.playersInGame = players;
				print ("game ready..");
				if (OnGameIsReady != null)
					OnGameIsReady ();
			}
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

			//GameObject gameObject = GameObject.Find("Player_"+nextPlayer);
			GameObject gameObject = Instantiate (playerPrefab) as GameObject;
			gameObject.name = "Player_" + nextPlayer;
			gameObject.transform.parent = GameObject.Find ("Players").transform;
			PlayerController player = gameObject.GetComponent<PlayerController>();
			player.Device = inputDevice;
			players.Add( player );

			if (playtesting)
				return player;
			
			Text playerBox = GameObject.Find ("BoxPlayer_" + nextPlayer).transform.Find("Player"+nextPlayer+"/Press A").gameObject.GetComponent<Text>();
			playerBox.text = "Ok!";

			Image backgroundImage = GameObject.Find ("BoxPlayer_" + nextPlayer).gameObject.GetComponent<Image> ();
			backgroundImage.color = new Color (0.078f, 0.29f, 0.51f, 0.392f);

			return player;
		}

		return null;
	}
}
