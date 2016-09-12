using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : UnitySingleton <GameStateManager>
{
	public List <PlayerController> playersInGame = new List <PlayerController>();
	public PlayerController winner { get; set;}

	void OnEnable()
	{	
		DevicesManager.OnGameIsReady += GetPlayersList; //TEST
		DevicesManager.OnGameIsReady += SpawnPlayers; //TEST
		DevicesManager.OnGameIsReady += InitializeScore; //TEST

		TimeManager.OnGameIsStarted += GameIsStarted;
		TimeManager.OnGameIsStarted += CreateInitialWolf;

		TimeManager.OnGameIsOver += HighestScore;

	}

	void HighestScore ()
	{
		FindObjectOfType<ScoreManager> ().GetWinner (playersInGame);
		print ("The winner is " + winner.name);
	}

	void GetPlayersList (List<PlayerController> playersList)
	{
		for (int i = 0 ; i < playersList.Count ; i++)
		{
			playersInGame.Add (playersList [i]);
		}
	}

	void GameIsStarted()
	{
		TimeManager.Instance.isGameStarted = true;
		print ("Game is started!");
	}

	void CreateInitialWolf()
	{
		//JUST FOR PLAYTESTING
		FindObjectOfType<NewWolfManager> ().CreateRandomWolf ();
	}

	void SpawnPlayers(List<PlayerController> playersList)
	{
		PlayerSpawnerManager.Instance.InitialPlayerSpawn (playersList);
	}

	void InitializeScore(List<PlayerController> playersList)
	{
		FindObjectOfType<ScoreManager> ().InitializeScore (playersList);

	}

	void OnDisable()
	{
		DevicesManager.OnGameIsReady -= SpawnPlayers; //TEST
		DevicesManager.OnGameIsReady -= GetPlayersList; //TEST

		TimeManager.OnGameIsStarted -= GameIsStarted;
		TimeManager.OnGameIsStarted -= CreateInitialWolf;
		//TimeManager.OnGameIsStarted -= SpawnPlayers;

	}
}
