using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameStateManager : UnitySingleton <GameStateManager>
{
	public List <PlayerController> playersInGame { get; private set;}

	public List <GameObject> dontDestroy = new List <GameObject>();

	DevicesManager devices;
	TimeManager time;
	ScoreManager score;
	GameOverManager gameOver;
	NewWolfManager newWolf;
	SceneManagerUtils scene;

	public enum GameState
	{
		GameReady,
		GameStarted,
		GameOver
	}

	void SetGameState (GameState newState)
	{
		switch (newState)
		{
		case GameState.GameReady:
			devices.lookingForPlayers = false;
			time.enabled = true;
			score.enabled = true;
			newWolf.enabled = true;
			break;

		case GameState.GameStarted:
			time.isGameStarted = true;
			break;

		case GameState.GameOver:
			time.enabled = false;
			score.GetWinner ();
			score.enabled = false;
			newWolf.enabled = false;
			gameOver.enabled = true;
			break;
		}
	}

	void Start()
	{	
		devices = GetComponent<DevicesManager> ();
		time = GetComponent<TimeManager> ();
		score = GetComponent<ScoreManager> ();
		gameOver = GetComponent<GameOverManager> ();
		newWolf = GetComponent<NewWolfManager> ();
		scene = transform.Find ("SceneManager").GetComponent<SceneManagerUtils> ();
		dontDestroy.Add (this.gameObject);
	}

	void OnEnable()
	{	
		DevicesManager.OnGameIsReady += GoToGameScene;

		TimeManager.OnGameIsStarted += GameIsStarted;

		TimeManager.OnGameIsOver += CallGameOverManager;
	}

	public void GoToGameScene()
	{
		playersInGame = GetComponent<DevicesManager> ().players;

		StartCoroutine (LoadGameScene ("Main Menu", "Game Scene"));
	}

	IEnumerator LoadGameScene(string currentScene, string sceneToLoad)
	{
		if (SceneManager.GetActiveScene ().name != "Game Scene")	
		{
			scene.LoadSceneWithPersist (currentScene, sceneToLoad, dontDestroy);

			while (!scene.sceneLoaded) 
			{
				yield return null;
			}
		}

		SetGameState (GameState.GameReady);

		FindObjectOfType<PlayerSpawnerManager>().InitialPlayerSpawn (playersInGame);
	}
		
	void GameIsStarted()
	{
		SetGameState (GameState.GameStarted);
	}

	void CallGameOverManager()
	{
		SetGameState (GameState.GameOver);

		for(int i = 0 ; i < playersInGame.Count ; i++)
		{
			PlayerController player = playersInGame [i];
			player.isWolf = false;
			player.Device.StopVibration ();
		}
	}

	void OnDisable()
	{
		DevicesManager.OnGameIsReady -= GoToGameScene;

		TimeManager.OnGameIsStarted -= GameIsStarted;

		TimeManager.OnGameIsOver -= CallGameOverManager;

	}
}
