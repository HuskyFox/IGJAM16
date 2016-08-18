using InControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	int minPlayers = 2;
	const int maxPlayers = 4;

	[HideInInspector]public List<Player> players = new List<Player>( maxPlayers );

	List<Vector3> playerPositions;

	public GameObject playerPrefab;

	public bool isGameStarted = false;
	public int currentWolfIndex;
	public bool isWolfCreated = false;

	public bool isNumberOfPlayersOk = false;
	public bool isControllerRegistrationActivated = false;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
		for (int i = 1; i <= maxPlayers; i++) {
			DontDestroyOnLoad(GameObject.Find("Player_"+i));
		}
		DontDestroyOnLoad(GameObject.Find("InControl"));

	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		var inputDevice = InputManager.ActiveDevice;

		if(SceneManager.GetActiveScene().name=="MainMenu") {
			if(isNumberOfPlayersOk) {
				if (inputDevice.Command.IsPressed) {
					SceneManager.LoadScene("Demo Scene");
					SoundManager.instance.StopAmbianceField ();
				}

			}
		}
		if (/*!isGameStarted && */isControllerRegistrationActivated) {
			if (JoinButtonWasPressedOnDevice (inputDevice)) {
				if (ThereIsNoPlayerUsingDevice (inputDevice)) {
					AssignDeviceToPlayer (inputDevice);
					SoundManager.instance.PlayRandomSheepBaa();
				}
				if (players.Count >= minPlayers && players.Count <= maxPlayers) {
				//	isGameStarted = true;
				}
			}
		} else {
			if(!isWolfCreated)
				CreateRandomWolf ();
		}
		if (Input.GetKey (KeyCode.Space)) {
			CreateRandomWolf ();
		}

	}

	void CreateRandomWolf() {
		MakeEveryoneASheep ();
		currentWolfIndex = CreateNewRandomNumber ();
		var wolf = GameObject.Find("Player_"+currentWolfIndex).GetComponent<Player>();
		wolf.MakeWolf ();
		isWolfCreated = true;
	}

	void MakeEveryoneASheep() {
		for(int i=1;i<=players.Count;i++) {
			GameObject.Find("Player_"+i).GetComponent<Player>().MakeSheep();
		}
	}

	int CreateNewRandomNumber() {
		int randomPlayerIndex = 0;
		do {
			randomPlayerIndex = Random.Range (1, players.Count);
		} while( randomPlayerIndex==currentWolfIndex );

		return randomPlayerIndex;
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


	void AssignDeviceToPlayer( InputDevice inputDevice )
	{
		if (players.Count < maxPlayers) {

			int nextPlayer = players.Count + 1;

			GameObject gameObject = GameObject.Find("Player_"+nextPlayer);
			Player player = gameObject.GetComponent<Player> ();
			player.Device = inputDevice;
			players.Add (player);

			Text playerBox = GameObject.Find ("BoxPlayer" + nextPlayer).transform.Find("Press A").gameObject.GetComponent<Text>();
			playerBox.text = "Ok!";
		} 

		if(players.Count >= minPlayers && players.Count<=maxPlayers){
			isNumberOfPlayersOk = true;
		}
	}

	public void ActivateControllerRegistration() {
		isControllerRegistrationActivated = true;
	}

	public void ChangeMainMenu(GameObject defaultHighlightedButton) {
		EventSystem.current.SetSelectedGameObject ( defaultHighlightedButton );
	}
}
