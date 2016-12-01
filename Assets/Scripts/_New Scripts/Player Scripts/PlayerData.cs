using UnityEngine;
using System.Collections;
using InControl;

public class PlayerData : MonoBehaviour
{
	public int playerIndex;
	public InputDevice controller;
	public int scoreKeeper;

	[SerializeField] PlayerActions actions;
	[SerializeField] ControllerVibration vibration;
	[SerializeField] PlayerMovement movement;

	void Start()
	{
		Invoke ("EnableMovement", 1f);
	}

	void EnableMovement()
	{
		movement.enabled = true;
	}

//	public void MakeWolf()
//	{
//		actions.isWolf = true;
//		vibration.isWolf = true;
//
//		tag = "Wolf";
//	}
//
//	public void MakeSheep()
//	{
//		actions.isWolf = false;
//		vibration.isWolf = false;
//
//		tag = "PlayerSheep";
//	}
//
//	public void ActivatePlayer()
//	{
//		actions.enabled = true;
//		movement.enabled = true;
//		vibration.enabled = true;
//	}
//
//	public void DeactivatePlayer()
//	{
//		actions.enabled = false;
//		movement.enabled = false;
//		vibration.enabled = false;
//	}

	public enum PlayerState
	{
		Sheep,
		Wolf,
		Killed
		//Paused
	}

	public void SetPlayerState (PlayerState newState)
	{
		switch (newState)
		{
		case PlayerState.Sheep:
			actions.isWolf = false;
			vibration.Stop ();
			tag = "PlayerSheep";
			break;
		case PlayerState.Wolf:
			actions.isWolf = true;
			vibration.WolfVibration ();
			tag = "Wolf";
			break;
		case PlayerState.Killed:
			vibration.KillVibration ();
			break;
//		case PlayerState.Paused:
//			actions.PauseToggle ();
//			movement.PauseToggle ();
//			vibration.PauseToggle ();
//			break;
		}
	}

	public void PauseToggle()
	{
		actions.PauseToggle ();
		movement.PauseToggle ();
		vibration.PauseToggle ();
	}
}
