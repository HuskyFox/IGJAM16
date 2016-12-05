using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/* This script handles the different game states :
 * - ready : spawn the registered players and the NPSheep, set the scores, start initial countdown + wolf countdown.
 * - started : start the game timer.
 * - over : get the winner(s) and disable the manager scripts, change the music.
 * - paused/unpaused : pause/unpause the timers, the players and the NPSheep.
 * 
 * It's also in charge of allowing to playtest the game scene, by ticking the "playtesting" bool in the inspector.
 * 
 * + handles the restart function without having to reload the scene.*/
 
public class GameStateManager : MonoBehaviour
{
	public List <PlayerData> playersInGame = new List<PlayerData> ();

	[SerializeField] private bool _playtesting = false;
	[SerializeField] private GameObject _playtestInit;
	[SerializeField] private PlayerSpawnerManager _players;
	[SerializeField] private NPSheepSpawner _npSheepSpawner;
	[SerializeField] private GameTimeUI _timer;
	[SerializeField] private GameObject _countdownUI;
	[SerializeField] private WolfCountdownUI _wolfUI;
	[SerializeField] private HowlCoolDownUI _howlUI;
	[SerializeField] private PauseManager _pause;
	private GameData _gameData;
	private ScoreManager _score;
	private NewWolfManager _newWolf;

	public enum GameState
	{
		GameReady,
		GameStarted,
		GameOver,
		GamePaused,
		GameUnpaused
	}

	void SetGameState (GameState newState)
	{
		switch (newState)
		{
		case GameState.GameReady:
			print ("Number of players:" + playersInGame.Count);
			_countdownUI.SetActive (true);
			_score.enabled = true;
			_score.InitializeScore ();
			_newWolf.enabled = true;
			_npSheepSpawner.SpawnNPSheep ();
			_pause.canPause = false;
			SoundManager.Instance.PlayGameMusic ();
			break;

		case GameState.GameStarted:
			_timer.StartGameTimer ();
			SoundManager.Instance.PlayRandomSheepBaa ();
			_pause.canPause = true;
			break;

		case GameState.GameOver:
			_score.GetWinner ();
			_score.enabled = false;
			_newWolf.enabled = false;
			_pause.canPause = false;
			SoundManager.Instance.PlayGameOverMusic ();
			SoundManager.Instance.CancelInvoke ();
			break;

		case GameState.GamePaused:
			PausePlayersToggle ();
			PauseNPSheepToggle ();
			PauseUIToggle ();
			SoundManager.Instance.PauseAudioToggle ();
			break;

		case GameState.GameUnpaused:
			Invoke ("PausePlayersToggle", 0.1f);
			PauseNPSheepToggle ();
			PauseUIToggle ();
			SoundManager.Instance.PauseAudioToggle ();
			break;
		}
	}

	void Awake()
	{
		_score = GetComponent<ScoreManager> ();
		_newWolf = GetComponent<NewWolfManager> ();

		if(!_playtesting) 
		{
			_gameData = GameObject.Find ("GameData").GetComponent<GameData> ();
			StartCoroutine (InitGame ());
		}
	}

	IEnumerator InitGame()
	{
		//populate the list and spawn the players
		playersInGame = _players.InitialPlayerSpawnTest (_gameData.registeredPlayers);
		yield return new WaitForEndOfFrame();
		SetGameState (GameState.GameReady);
	}

	#if (UNITY_EDITOR)
	/* The playtesing initialization of the game has to be done in the start function and not in the awake function,
	 * because it takes some frames to find the GameData.
	 * (at least that's my conclusion after trying to put it in the awake function) */
	void Start()
	{	
		if (_playtesting) 
		{
			_gameData = GameObject.Find ("GameData").GetComponent<GameData> ();
			StartCoroutine (InitPlaytest ());
		}
	}

	IEnumerator InitPlaytest()
	{	
		//the playtest gameobject has a script attached that allows a controller registration without having to go through the main menu.
		GameObject playtest = Instantiate (_playtestInit) as GameObject;

		//wait for the playtester(s)'s input
		while(!playtest.GetComponent<PlaytestRegistration>().readyToPlaytest)
		{
			yield return null;
		}

		StartCoroutine (InitGame ());
	}
	#endif

	void OnEnable()
	{	
		InitialCountdownUI.OnGameIsStarted += GameIsStarted;
		GameTimeUI.OnGameIsOver += GameOver;
		MenuFunctions.OnRestartGamePressed += RestartGame;
		PauseManager.OnGamePaused += PauseGame;
		PauseManager.OnGameUnpaused += UnpauseGame;
	}
		
	void GameIsStarted()
	{
		SetGameState (GameState.GameStarted);
	}

	void GameOver()
	{
		SetGameState (GameState.GameOver);
	}

	void PauseGame()
	{
		SetGameState (GameState.GamePaused);
	}

	void UnpauseGame()
	{
		SetGameState (GameState.GameUnpaused);
	}

	void PausePlayersToggle()
	{
		//we could do that directly in the PlayerData script by subscribing them to event, it might be less expensive than calling GetComponent.
		for(int i = 0 ; i < playersInGame.Count ; i++)
		{
			playersInGame [i].GetComponent<PlayerData> ().PauseToggle ();
		}
	}

	void PauseNPSheepToggle()
	{
		//we could do that directly in the NPSheep script by subscribing them to event, 
		//it might be less expensive than calling GetComponent many times.

		List <GameObject> npSheepToStop = _npSheepSpawner.npSheepInGame;
		foreach(GameObject npSheep in npSheepToStop)
		{
			NPSheep behavior = npSheep.GetComponent<NPSheep> ();
			behavior.PauseToggle ();
		}
	}

	void PauseUIToggle()
	{
		_timer.currentlyPlaying = !_timer.currentlyPlaying;
		_wolfUI.currentlyPlaying = !_wolfUI.currentlyPlaying;
		_newWolf.currentlyPlaying = !_newWolf.currentlyPlaying;
		_howlUI.currentlyPlaying = !_howlUI.currentlyPlaying;
	}

	void RestartGame()
	{
		ClearGame ();
	
		StartCoroutine (InitGame ());
	}

	void ClearGame()
	{
		//destroy players
		foreach(PlayerData player in playersInGame)
		{
			DestroyObject (player.gameObject);
		}

		playersInGame.Clear ();

		//reset game timer
		_timer.ResetTimer ();

		_newWolf.enabled = false;
	}

	void OnDisable()
	{
		InitialCountdownUI.OnGameIsStarted += GameIsStarted;
		MenuFunctions.OnRestartGamePressed -= RestartGame;
		GameTimeUI.OnGameIsOver -= GameOver;
		PauseManager.OnGamePaused -= PauseGame;
		PauseManager.OnGameUnpaused -= UnpauseGame;
	}
}
