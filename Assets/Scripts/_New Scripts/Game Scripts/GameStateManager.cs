using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameStateManager : MonoBehaviour
{
	[SerializeField] bool playtesting = false;
	[SerializeField] GameObject playtestInit;

	public List <PlayerData> playersInGame = new List<PlayerData> ();

	[SerializeField] PlayerSpawnerManager players;
	[SerializeField]NPSheepSpawner npSheepSpawner;
	[SerializeField] TimeUI timeUI;
	GameData gameData;
	TimeManager time;
	ScoreManager score;
	NewWolfManager newWolf;

	public enum GameState
	{
		GameReady,
		GameStarted,
		GameOver,
		GamePaused
	}

	void SetGameState (GameState newState)
	{
		switch (newState)
		{
		case GameState.GameReady:
			print ("Number of players:" + playersInGame.Count);
			time.enabled = true;
			time.ResetTime ();
			score.enabled = true;
			score.InitializeScore ();
			newWolf.enabled = true;
			npSheepSpawner.SpawnNPSheep ();
			SoundManager.Instance.PlayGameMusic ();
			break;

		case GameState.GameStarted:
			time.isGameStarted = true;
			ActivatePlayers ();
			SoundManager.Instance.PlayRandomSheepBaa ();
			break;

		case GameState.GameOver:
			StopPlayers ();
			timeUI.HideWolfCountdown ();
			time.isGameStarted = false;
			score.GetWinner ();
			score.enabled = false;
			newWolf.enabled = false;
			SoundManager.Instance.PlayGameOverMusic ();
			SoundManager.Instance.CancelInvoke ();
			break;

		case GameState.GamePaused:
			time.isGameStarted = false;
			SoundManager.Instance.CancelInvoke ();
			StopPlayers ();
			break;
		}
	}

	void Awake()
	{
		time = GetComponent<TimeManager> ();
		score = GetComponent<ScoreManager> ();
		newWolf = GetComponent<NewWolfManager> ();

		if(!playtesting) 
		{
			gameData = GameObject.Find ("GameData").GetComponent<GameData> ();
			StartCoroutine (InitGame ());
		}
	}

	IEnumerator InitGame()
	{
		playersInGame = players.InitialPlayerSpawnTest (gameData.registeredPlayers);
		yield return new WaitForEndOfFrame();
		SetGameState (GameState.GameReady);
	}

	void Start()
	{	
		if (playtesting) 
		{
			gameData = GameObject.Find ("GameData").GetComponent<GameData> ();
			StartCoroutine (InitPlaytest ());
		}
	}

	IEnumerator InitPlaytest()
	{
		GameObject playtest = Instantiate (playtestInit) as GameObject;
		while(!playtest.GetComponent<PlaytestRegistration>().readyToPlaytest)
		{
			yield return null;
		}

		playersInGame = players.InitialPlayerSpawnTest (gameData.registeredPlayers);
		SetGameState (GameState.GameReady);
	}

	void OnEnable()
	{	
		TimeManager.OnGameIsStarted += GameIsStarted;

		TimeManager.OnGameIsOver += GameOver;

		MenuFunctions.OnRestartGamePressed += RestartGame;

		PauseManager.OnGamePaused += PauseGame;

		PauseManager.OnGameUnpaused += GameIsStarted;
	}

	void RestartGame()
	{
		foreach(PlayerData player in playersInGame)
		{
			DestroyObject (player.gameObject);
		}

		playersInGame.Clear ();

		playersInGame = players.InitialPlayerSpawnTest (gameData.registeredPlayers);
		SetGameState (GameState.GameReady);
		newWolf.CreateRandomWolf ();
	}
		
	void GameIsStarted()
	{
		SetGameState (GameState.GameStarted);
	}

	void GameOver()
	{
		SetGameState (GameState.GameOver);
	}

	void StopPlayers()
	{
		for(int i = 0 ; i < playersInGame.Count ; i++)
		{
			playersInGame [i].GetComponent<PlayerData> ().DeactivatePlayer ();
		}
	}

	void ActivatePlayers()
	{
		for(int i = 0 ; i < playersInGame.Count ; i++)
		{
			playersInGame [i].GetComponent<PlayerData> ().ActivatePlayer ();
		}
	}

	void PauseGame()
	{
		SetGameState (GameState.GamePaused);
		//StopNPSheep ();
	}

	void StopNPSheep()
	{
//		List <GameObject> npSheepToStop = npSheepSpawner.npSheepInGame;
//		foreach(GameObject npSheep in npSheepToStop)
//		{
////			NPSheep behavior = npSheep.GetComponent<NPSheep> ();
////			behavior.StopMoving ();
//
//		}
	}

	void OnDisable()
	{
		TimeManager.OnGameIsStarted -= GameIsStarted;

		MenuFunctions.OnRestartGamePressed -= RestartGame;

		TimeManager.OnGameIsOver -= GameOver;

		PauseManager.OnGamePaused -= PauseGame;

		PauseManager.OnGameUnpaused -= GameIsStarted;
	}
}
