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

	public void MakeWolf()
	{
		actions.isWolf = true;
		vibration.isWolf = true;

		tag = "Wolf";
	}

	public void MakeSheep()
	{
		actions.isWolf = false;
		vibration.isWolf = false;

		tag = "PlayerSheep";
	}

	public void ActivatePlayer()
	{
		actions.enabled = true;
		movement.enabled = true;
		vibration.enabled = true;
	}

	public void DeactivatePlayer()
	{
		actions.enabled = false;
		movement.enabled = false;
		vibration.enabled = false;
	}
}
