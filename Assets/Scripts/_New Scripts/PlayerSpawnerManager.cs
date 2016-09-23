using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerSpawnerManager : UnitySingleton <PlayerSpawnerManager>
{
	public float spawnHeight = 4f;
	public int numberOfPlayers;
	private string playerIndex;

	void Start()
	{
		GameStateManager.Instance.dontDestroy.Add (this.gameObject);
	}

	//The 4 players are hidden at the beginning (the "Hip" GameObject of the sheep is inactive in the inspector).
	//This function finds the number of players to spawn from the registered devices list,
	//and only spawns the amount of players that are registered (the others stay hidden).
	public void InitialPlayerSpawn(List<PlayerController> playersToSpawn)
	{
		numberOfPlayers = playersToSpawn.Count;

		for(int i = 0 ; i < numberOfPlayers ; i++)
		{
			PlayerController playerToSpawn = playersToSpawn [i];

			//finds the "hip" GameObject to set it active and make the player appear.
			GameObject playerShape = playerToSpawn.transform.Find ("Sheep/Hip").gameObject;

			//finds the right position and rotation corresponding to the index of the player
			playerIndex = playerToSpawn.name.Replace ("Player_", "");
			Vector3 spawnPosition = GameObject.Find ("SpawnPositionPlayer_" + playerIndex).transform.position;
			Quaternion spawnRotation = GameObject.Find ("SpawnPositionPlayer_" + playerIndex).transform.rotation;
			spawnPosition.y = spawnHeight;	//if we want to change the spawn height in the inspector.

			//assigns the position and the rotation, and set the player active.
			playerToSpawn.transform.position = spawnPosition;
			playerToSpawn.transform.rotation = spawnRotation;

			playerToSpawn.GetComponent<PlayerController> ().enabled = true;	
			playerToSpawn.GetComponent<Rigidbody> ().useGravity = true;

			playerShape.SetActive (true);
			//PARTICLES ?
			//ground indicator ?
		}
	}

	//Almost the same as InitialPlayerSpawn (just without the SetActive because the kill function doesn't set it inactive).
	public void RespawnPlayer(PlayerController playerToRespawn)
	{
		//finds the right position and rotation corresponding to the index of the player
		playerIndex = playerToRespawn.name.Replace ("Player_", "");
		Vector3 respawnPosition = GameObject.Find ("SpawnPositionPlayer_" + playerIndex).transform.position;
		Quaternion respawnRotation = GameObject.Find ("SpawnPositionPlayer_" + playerIndex).transform.rotation;
		respawnPosition.y = spawnHeight; //if we want to change the spawn height in the inspector.

		//assigns the position and the rotation to the player.
		playerToRespawn.transform.position = respawnPosition;
		playerToRespawn.transform.rotation = respawnRotation;
	}
}
