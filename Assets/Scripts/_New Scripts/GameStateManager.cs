using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : UnitySingleton <GameStateManager>
{
	public List <PlayerController> playersInGame { get; set; }
	public static bool gameOn;

	void OnEnable()
	{	
		DevicesManager.OnGameIsReady += SpawnPlayers; //TEST
		DevicesManager.OnGameIsReady += InitializeScore; //TEST

		TimeManager.OnGameIsStarted += GameIsStarted;
		TimeManager.OnGameIsStarted += CreateInitialWolf;

		TimeManager.OnGameIsOver += HighestScore;
		TimeManager.OnGameIsOver += CallGameOverManager;
	}

	void CallGameOverManager()
	{
		gameOn = false;
		GetComponent <GameOverManager> ().enabled = true;
	}

	void HighestScore ()
	{
		FindObjectOfType<ScoreManager> ().GetWinner (playersInGame);
	}

	void GameIsStarted()
	{
		gameOn = true;
		TimeManager.Instance.isGameStarted = true;
		print ("Game is started!");
	}

	void CreateInitialWolf()
	{
		//JUST FOR PLAYTESTING
		GetComponent <NewWolfManager> ().CreateRandomWolf ();
	}

	void SpawnPlayers()
	{
		PlayerSpawnerManager.Instance.InitialPlayerSpawn (playersInGame);
	}

	void InitializeScore()
	{
		GetComponent <ScoreManager> ().InitializeScore (playersInGame);

	}

	void OnDisable()
	{
		DevicesManager.OnGameIsReady -= SpawnPlayers; //TEST

		TimeManager.OnGameIsStarted -= GameIsStarted;
		TimeManager.OnGameIsStarted -= CreateInitialWolf;
		TimeManager.OnGameIsOver -= HighestScore;

		TimeManager.OnGameIsOver -= CallGameOverManager;


	}
}
