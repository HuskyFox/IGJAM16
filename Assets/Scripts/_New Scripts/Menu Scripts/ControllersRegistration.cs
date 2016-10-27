using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine.UI;

//This script is in charge of registering the input from the controllers. Every time a new input device is detected,
//it triggers an event listened by the script "GameData" in the NeverUnload scene. 
public class ControllersRegistration : MonoBehaviour 
{
	public int minPlayers = 1;
	const int maxPlayers = 4;

	[SerializeField]
	GameObject[] playerBoxes = new GameObject[4];
	[SerializeField]
	Color boxColor;

	//the list of input devices, which cannot contain more elements than the maximum amount of players.
	[HideInInspector]public List<InputDevice> controllers = new List<InputDevice>( maxPlayers );

	public delegate void NewPlayerRegistered (int playerIndex, InputDevice controller);
	public static event NewPlayerRegistered OnNewPlayerRegistered;

	void OnEnable ()
	{
		StartReturnButtons.OnReturnToMenu += ResetPlayerBoxesAndList;
	}

	void Update()
	{
		if (controllers.Count < maxPlayers) 
		{
			var inputDevice = InputManager.ActiveDevice;

			if (JoinButtonWasPressedOnDevice (inputDevice))
			{
				if (ThereIsNoPlayerUsingDevice (inputDevice)) 
				{
					AssignDeviceToPlayer (inputDevice);
				}
			}
		}
	}

	//check if A was pressed on any device.
	bool JoinButtonWasPressedOnDevice( InputDevice inputDevice )
	{
		return inputDevice.Action1.WasPressed;
	}

	//check if the device is already in the list.
	bool ThereIsNoPlayerUsingDevice( InputDevice inputDevice )
	{
		return FindPlayerUsingDevice( inputDevice ) == null;
	}

	InputDevice FindPlayerUsingDevice( InputDevice inputDevice )
	{
		for (var i = 0; i < controllers.Count; i++)
		{
			var device = controllers[i];
			if (device == inputDevice)
				return device;
		}
		return null;
	}

	//If the device is not in the list yet, this function triggers an event (see GameData),
	//adds the device to the list,
	//and gives visual and sound feedback that the player is registered.
	InputDevice AssignDeviceToPlayer( InputDevice inputDevice )
	{
		if (controllers.Count < maxPlayers)
		{
			int newPlayerIndex = controllers.Count + 1;

			if (OnNewPlayerRegistered != null)
				OnNewPlayerRegistered (newPlayerIndex, inputDevice);

			//GameObject gameObject = Instantiate (playerPrefab) as GameObject;
			//gameObject.name = "Player_" + nextPlayer;
			//gameObject.transform.parent = GameObject.Find ("Players").transform;
			//PlayerController player = gameObject.GetComponent<PlayerController>();
			//player.Device = inputDevice;
			controllers.Add(inputDevice);

			GameObject box = playerBoxes [newPlayerIndex - 1];

			Text playerBox = box.transform.Find("Player"+newPlayerIndex+"/Press A").GetComponent<Text>();
			playerBox.text = "Ok!";

			AudioSource boxAudioSource = box.GetComponent<AudioSource> ();
			SoundManager.Instance.PlayRandomSheepBaa (boxAudioSource);

			Image backgroundImage = box.GetComponent<Image> ();
			backgroundImage.color = new Color (0.078f, 0.29f, 0.51f, 0.392f);

			return inputDevice;
		}

		return null;
	}

	//If a player presses B to return to the menu, the controllers list is cleared, and the boxes are reset.
	void ResetPlayerBoxesAndList()
	{
		for(int i = 0 ; i < playerBoxes.Length ; i++)
		{
			GameObject box = playerBoxes [i];
			Image backgroundImage = box.GetComponent<Image> ();
			backgroundImage.color = boxColor;

			Text pressA = box.transform.Find ("Player" + (i + 1) + "/Press A").GetComponent<Text> ();
			pressA.text = "Press <color=#008000ff>A</color>";
		}

		controllers.Clear ();
	}

	void OnDisable()
	{
		StartReturnButtons.OnReturnToMenu -= ResetPlayerBoxesAndList;

	}
}
