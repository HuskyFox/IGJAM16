using InControl;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	const int maxPlayers = 4;

	List<Player> players = new List<Player>( maxPlayers );

	List<Vector3> playerPositions;

	public GameObject playerPrefab;

	public bool isGameStarted = false;
	private Text numberOfPlayersText;


	// Use this for initialization
	void Start () {
		numberOfPlayersText = GameObject.Find ("NumberOfPlayersText").GetComponent<Text> ();
		playerPositions = new List<Vector3>() {
			GameObject.Find("Plane1").transform.position,
			GameObject.Find("Plane2").transform.position,
			GameObject.Find("Plane3").transform.position,
			GameObject.Find("Plane4").transform.position,
		};
	}
	
	// Update is called once per frame
	void Update () {
		var inputDevice = InputManager.ActiveDevice;

		if (!isGameStarted) {
			if (JoinButtonWasPressedOnDevice (inputDevice)) {
				if (ThereIsNoPlayerUsingDevice (inputDevice)) {
					CreatePlayer (inputDevice);
				}
				if (players.Count == maxPlayers) {
					isGameStarted = true;
				}
			}
		} else {
			CreateRandomWolf ();
		}

	}

	void CreateRandomWolf() {
		int randomPlayerIndex = Random.Range (1, 4);
		var wolf = GameObject.Find("Player_"+randomPlayerIndex).GetComponent<Player>();
		wolf.MakeWolf ();
	}


	bool JoinButtonWasPressedOnDevice( InputDevice inputDevice )
	{
		return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
	}


	Player FindPlayerUsingDevice( InputDevice inputDevice )
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


	void OnDeviceDetached( InputDevice inputDevice )
	{
		var player = FindPlayerUsingDevice( inputDevice );
		if (player != null)
		{
			RemovePlayer( player );
		}
	}


	void RemovePlayer( Player player )
	{
		playerPositions.Insert( 0, player.transform.position );
		players.Remove( player );
		player.Device = null;
		Destroy( player.gameObject );
	}


	void CreatePlayer( InputDevice inputDevice )
	{
		if (players.Count < maxPlayers)
		{
			// Pop a position off the list. We'll add it back if the player is removed.
			var playerPosition = playerPositions[0];
			playerPositions.RemoveAt( 0 );

			var gameObject = (GameObject) Instantiate( playerPrefab, playerPosition, Quaternion.identity );
			var player = gameObject.GetComponent<Player>();
			player.name = "Player_"+players.Count;
			player.Device = inputDevice;
			Vector3 pos = player.transform.position;
			pos.y = 1;
			player.transform.position = pos;
			players.Add( player );
			numberOfPlayersText.text = "Number of players: " + players.Count;
		}

	}
}
