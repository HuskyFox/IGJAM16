using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PlaytestRegistration : MonoBehaviour 
{
	const int maxPlayers = 4;

	[HideInInspector]public bool readyToPlaytest = false;

	[HideInInspector]public List<InputDevice> controllers = new List<InputDevice>( maxPlayers );

	public delegate void NewPlayerRegisteredForPlaytest (int playerIndex, InputDevice controller);
	public static event NewPlayerRegisteredForPlaytest OnNewPlayerRegisteredForPlaytest;

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
					print ("New player registered.");
				}
			}
		}

		if (InputManager.ActiveDevice.Action3.WasPressed)
			readyToPlaytest = true;
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
	//and adds the device to the list.
	InputDevice AssignDeviceToPlayer( InputDevice inputDevice )
	{
		if (controllers.Count < maxPlayers)
		{
			int newPlayerIndex = controllers.Count + 1;

			if (OnNewPlayerRegisteredForPlaytest != null)
				OnNewPlayerRegisteredForPlaytest (newPlayerIndex, inputDevice);
			
				controllers.Add(inputDevice);

			return inputDevice;
		}

		return null;
	}
}
