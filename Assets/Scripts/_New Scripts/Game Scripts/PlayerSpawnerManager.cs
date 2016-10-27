using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class PlayerSpawnerManager : MonoBehaviour
{
	[SerializeField]
	GameObject playerPrefab;
	public float spawnHeight = 4f;
	public int numberOfPlayers;
	public GameObject spawnParticles;
	private int playerIndex;

	[SerializeField]
	ScoreManager score;

	private List<PlayerData> playersInGame = new List<PlayerData>();

	//The 4 players are hidden at the beginning (the "Hip" GameObject of the sheep is inactive in the inspector).
	//This function finds the number of players to spawn from the registered devices list,
	//and only spawns the amount of players that are registered (the others stay hidden).

	public List<PlayerData> InitialPlayerSpawnTest (Dictionary <int, InputDevice> playersToSpawn)
	{
		foreach (KeyValuePair <int, InputDevice> players in playersToSpawn)
		{
			playerIndex = players.Key;

			GameObject playerToSpawn = Instantiate (playerPrefab) as GameObject;
			playerToSpawn.name = "Player_" + playerIndex;
			playerToSpawn.transform.parent = transform;

			PlayerData playerData = playerToSpawn.GetComponent<PlayerData> ();
			playerData.playerIndex = playerIndex;
			playerData.controller = players.Value;

			//finds the right position and rotation corresponding to the index of the player
			Vector3 spawnPosition = GameObject.Find ("SpawnPositionPlayer_" + playerIndex).transform.position;
			Quaternion spawnRotation = GameObject.Find ("SpawnPositionPlayer_" + playerIndex).transform.rotation;
			spawnPosition.y = spawnHeight;	//if we want to change the spawn height in the inspector.

			//assigns the position and the rotation, and set the player active.
			playerToSpawn.transform.position = spawnPosition;
			playerToSpawn.transform.rotation = spawnRotation;

			Image groundIndicator = playerToSpawn.transform.Find ("PlayerCanvas/GroundIndicator").GetComponent<Image> ();
			groundIndicator.color = new Color (Random.value, Random.value, Random.value, 0.70f);
			Invoke ("RemoveIndicator", 5f);

			//Instantiate (spawnParticles, playerShape.transform.position, Quaternion.identity);

			playersInGame.Add (playerData);
		}

		return playersInGame;
	}

	void RemoveIndicator()
	{
		GameObject[] playerIndicators = GameObject.FindGameObjectsWithTag ("Indicator");
		foreach (var playerIndicator in playerIndicators)
			playerIndicator.SetActive (false);
	}

	//Almost the same as InitialPlayerSpawn (just without the SetActive because the kill function doesn't set it inactive).
	public void RespawnPlayer(PlayerData playerToRespawn)
	{
		//finds the right position and rotation corresponding to the index of the player
		playerIndex = playerToRespawn.playerIndex;
		Vector3 respawnPosition = GameObject.Find ("SpawnPositionPlayer_" + playerIndex).transform.position;
		Quaternion respawnRotation = GameObject.Find ("SpawnPositionPlayer_" + playerIndex).transform.rotation;
		respawnPosition.y = spawnHeight; //if we want to change the spawn height in the inspector.

		//assigns the position and the rotation to the player.
		playerToRespawn.transform.position = respawnPosition;
		playerToRespawn.transform.rotation = respawnRotation;

	//	GameObject playerShape = playerToRespawn.transform.Find ("Sheep/Hip").gameObject;
	//	Instantiate (spawnParticles, playerShape.transform.position, Quaternion.identity);
	}
}
