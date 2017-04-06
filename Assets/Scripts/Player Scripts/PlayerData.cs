using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;

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
	[HideInInspector]public Text indexLabel;	//for the alternative to vibrations...

	[SerializeField] private bool _wolfColorLabel = false;	//set to true in the inspector if we want the label of the wolf player to be red (alternative to vibrations).
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
			SheepColor ();
			tag = "PlayerSheep";
			break;
		case PlayerState.Wolf:
			_actions.isWolf = true;
			_actions.canHowl = true;
			_vibration.WolfVibration ();
			WolfColor ();
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

	void WolfColor()
	{
		if(_wolfColorLabel)
			indexLabel.color = Color.red;
	}

	void SheepColor()
	{
		if(_wolfColorLabel)
			indexLabel.color = Color.white;
	}
}
