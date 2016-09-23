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
			time.ResetTime ();
			score.enabled = true;
			newWolf.enabled = true;
			SoundManager.Instance.PlayGameMusic ();
			break;

		case GameState.GameStarted:
			time.isGameStarted = true;
			SoundManager.Instance.PlayRandomSheepBaa ();
			break;

		case GameState.GameOver:
			time.isGameStarted = false;
			score.GetWinner ();
			score.enabled = false;
			newWolf.enabled = false;
			SoundManager.Instance.PlayGameOverMusic ();
			SoundManager.Instance.CancelInvoke ();
			break;
		}
	}

	void Start()
	{	
		devices = GetComponent<DevicesManager> ();
		time = GetComponent<TimeManager> ();
		score = GetComponent<ScoreManager> ();
		newWolf = GetComponent<NewWolfManager> ();
		scene = transform.Find ("SceneManager").GetComponent<SceneManagerUtils> ();
		dontDestroy.Add (this.gameObject);
	}

	void OnEnable()
	{	
		DevicesManager.OnGameIsReady += GoToGameScene;

		TimeManager.OnGameIsStarted += GameIsStarted;

		TimeManager.OnGameIsOver += GameOver;
	}

	public void GoToGameScene()
	{
		playersInGame = GetComponent<DevicesManager> ().players;
		SoundManager.Instance.StopMusic ();

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

	void GameOver()
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

		TimeManager.OnGameIsOver -= GameOver;

	}
}
