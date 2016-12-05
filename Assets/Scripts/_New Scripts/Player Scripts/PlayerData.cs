using UnityEngine;
using System.Collections;
using InControl;

/* The player control is divided into several scripts :
 * - PlayerData holds the important information about the player, and controls its state.
 * - PlayerMovement handles movement and animation.
 * - PlayerActions handles the actions (attack, howl...).
 * - ControllerVibration handles vibrations.*/
public class PlayerData : MonoBehaviour
{
	public int playerIndex;				//value assigned by the PlayerSpawnerManager script when the player is spawned.
	public InputDevice controller;		//value assigned by the PlayerSpawnerManager script when the player is spawned.
	public int scoreKeeper;				//value assigned and used by the ScoreManager script.

	[SerializeField] private PlayerActions _actions;
	[SerializeField] private PlayerMovement _movement;
	[SerializeField] private ControllerVibration _vibration;

	void Start()
	{
		//prevent movement for 1 sec when the game starts.
		Invoke ("EnableMovement", 1f);
	}

	//Also called by the PlayerSpawnerManager script after respawning a killed player.
	public void EnableMovement()
	{
		_movement.enabled = true;
	}

	public enum PlayerState
	{
		Sheep,
		Wolf
	}

	public void SetPlayerState (PlayerState newState)
	{
		switch (newState)
		{
		case PlayerState.Sheep:
			_actions.isWolf = false;
			_vibration.Stop ();
			tag = "PlayerSheep";
			break;
		case PlayerState.Wolf:
			_actions.isWolf = true;
			_actions.canHowl = true;
			_vibration.WolfVibration ();
			tag = "Wolf";
			break;
		}
	}

	//Called by the GameStateManager script (could be done here by subscribing this script to the Pause/Unpause events...)
	public void PauseToggle()
	{
		_actions.PauseToggle ();
		_movement.PauseToggle ();
		_vibration.PauseToggle ();
	}
}
