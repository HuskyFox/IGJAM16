using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

/* This script is in charge of spawning the players at the beginning of the game,
 * and to respawn the players when they are killed.*/
public class PlayerSpawnerManager : MonoBehaviour
{
	[SerializeField] private GameObject _playerPrefab;
	[SerializeField] private float _spawnHeight = 4f;
	[SerializeField] private Transform[] _spawnTransforms = new Transform[4];
	[SerializeField] private GameObject _spawnParticles;
	private int _playerIndex;
	private List<PlayerData> _playersInGame = new List<PlayerData>();
	private List<Image> _groundIndicators = new List<Image>();

	/* Called by the GameStateManager and being passed the dictionary of registered players,
	 * this function spawns the needed amount of players prefabs.*/
	public List<PlayerData> InitialPlayerSpawnTest (Dictionary <int, InputDevice> playersToSpawn)
	{
		foreach (KeyValuePair <int, InputDevice> player in playersToSpawn)
		{	
			//retrieve the player index from the dictionary key.
			_playerIndex = player.Key;

			//Instantiate the player prefab, rename it, and attach it to the players folder.
			GameObject playerToSpawn = Instantiate (_playerPrefab) as GameObject;
			playerToSpawn.name = "Player_" + _playerIndex;
			playerToSpawn.transform.parent = transform;

			//assign the index and the controller to the PlayerData script.
			PlayerData playerData = playerToSpawn.GetComponent<PlayerData> ();
			playerData.playerIndex = _playerIndex;
			playerData.controller = player.Value;

			//find the right position and rotation corresponding to the index of the player
			Vector3 spawnPosition = _spawnTransforms [_playerIndex - 1].transform.position;
			spawnPosition.y = _spawnHeight;	//if we want to change the spawn height in the inspector.
			playerToSpawn.transform.position = spawnPosition;
			playerToSpawn.transform.rotation = _spawnTransforms [_playerIndex - 1].transform.rotation;

			//Ground indicator to spot the players at the beginning.
			Image groundIndicator = playerToSpawn.transform.Find ("PlayerCanvas/GroundIndicator").GetComponent<Image> ();
			groundIndicator.color = new Color (Random.value, Random.value, Random.value, 0.70f);
			groundIndicator.enabled = true;
			_groundIndicators.Add (groundIndicator);
			Invoke ("RemoveIndicator", 3f);

			_playersInGame.Add (playerData);

			Instantiate (_spawnParticles, playerToSpawn.transform.position, Quaternion.identity);
		}

		return _playersInGame;
	}

	void RemoveIndicator()
	{
		foreach (Image indicator in _groundIndicators)
			indicator.enabled = false;
		_groundIndicators.Clear ();
	}

	//Almost the same as InitialPlayerSpawn.
	public void RespawnPlayer(PlayerData playerToRespawn)
	{
		//Get the player index.
		_playerIndex = playerToRespawn.playerIndex;

		//find the right position and rotation corresponding to the index of the player
		Vector3 spawnPosition = _spawnTransforms [_playerIndex - 1].transform.position;
		spawnPosition.y = _spawnHeight;	//if we want to change the spawn height in the inspector.
		playerToRespawn.transform.position = spawnPosition;
		playerToRespawn.transform.rotation = _spawnTransforms [_playerIndex - 1].transform.rotation;

		playerToRespawn.Invoke ("EnableMovement", 1f);

		Instantiate (_spawnParticles, playerToRespawn.transform.position, Quaternion.identity);
	}
}
