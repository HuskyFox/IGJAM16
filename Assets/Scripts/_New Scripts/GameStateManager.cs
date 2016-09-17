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

		GameOverManager.OnRestartGame += ReloadGameScene;
	}

	public void GoToGameScene()
	{
		//GetComponent<DevicesManager>().lookingForPlayers = false;
		playersInGame = GetComponent<DevicesManager> ().players;
		//SceneManager.MoveGameObjectToScene (this.gameObject, SceneManager.GetSceneByName ("Scene Manager"));
		//SceneManager.MoveGameObjectToScene (GameObject.Find("Players").gameObject, SceneManager.GetSceneByName ("Scene Manager"));

		if (SceneManager.GetActiveScene ().name != "Game Scene")
			StartCoroutine (LoadGameScene ("Main Menu", "Game Scene"));
		else if (SceneManager.GetActiveScene ().name == "Game Scene")
			ReloadGameScene ();
	}

	IEnumerator LoadGameScene(string currentScene, string sceneToLoad)
	{
		scene.LoadSceneWithPersist (currentScene, sceneToLoad, dontDestroy);

		while (!scene.sceneLoaded) 
		{
			yield return null;
		}

		SetGameState (GameState.GameReady);

		FindObjectOfType<PlayerSpawnerManager>().InitialPlayerSpawn (playersInGame);

		//GetComponent<TimeManager> ().enabled = true;
		//GetComponent <ScoreManager> ().enabled = true;
		//GetComponent <ScoreManager> ().enabled = true;
		//GetComponent<NewWolfManager> ().enabled = true;
	}
		
	void GameIsStarted()
	{
		SetGameState (GameState.GameStarted);
		//GetComponent<TimeManager>().isGameStarted = true;
//		GetComponent<NewWolfManager> ().enabled = true;
//		GetComponent <NewWolfManager> ().CreateRandomWolf ();
		print ("Game is started!");
	}

	void CallGameOverManager()
	{
		//FindObjectOfType<ScoreManager> ().GetWinner ();
		//FindObjectOfType<ScoreManager> ().enabled = false;
		SetGameState (GameState.GameOver);

		for(int i = 0 ; i < playersInGame.Count ; i++)
		{
			PlayerController player = playersInGame [i];
			player.isWolf = false;
			player.Device.StopVibration ();
		}
		//GetComponent<NewWolfManager> ().enabled = false;

		//GetComponent<TimeManager> ().enabled = false;
		//GetComponent <GameOverManager> ().enabled = true;
	}

	void ReloadGameScene()
	{
		SetGameState (GameState.GameReady);
		FindObjectOfType<PlayerSpawnerManager>().InitialPlayerSpawn (playersInGame);
//		GetComponent<TimeManager> ().enabled = true;
//		GetComponent <ScoreManager> ().enabled = true;
//		GetComponent<NewWolfManager> ().enabled = true;
	}

	void OnDisable()
	{
		DevicesManager.OnGameIsReady -= GoToGameScene;

		TimeManager.OnGameIsStarted -= GameIsStarted;

		TimeManager.OnGameIsOver -= CallGameOverManager;

		GameOverManager.OnRestartGame -= ReloadGameScene;
	}

	void OnDestroy()
	{
		print (gameObject.name + "was destroyed!");
	}
}
