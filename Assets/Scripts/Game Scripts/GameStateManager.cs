using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/* This script handles the different game states :
 * - ready : spawn the registered players and the NPSheep, set the scores, start initial countdown + wolf countdown.
 * - started : start the game timer.
 * - over : get the winner(s) and disable the manager scripts, change the music. Set players to sheep state (no vibrations).
 * - paused/unpaused : pause/unpause the timers, the players and the NPSheep.
 * 
 * It's also in charge of allowing to playtest the game scene, by enabling an alternate controller registration.
 * 
 * + handles the restart function without having to reload the scene.*/
 
public class GameStateManager : MonoBehaviour
{
	public List <PlayerData> playersInGame = new List<PlayerData> ();

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
	private HowlManager _howlMan;

	public enum GameState
	{
		GameReady,
		GameStarted,
		GameOver,
		GamePaused,
		GameUnpaused
	}

	public void SetGameState (GameState newState)
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
			_pause.canPause = false;	//disable pause function during countdown.
			SoundManager.Instance.PlayMusic ("Game");
			break;

		case GameState.GameStarted:
			_timer.StartGameTimer ();
			SoundManager.Instance.PlayRandomSheepBaa ();
			_pause.canPause = true;		//enable pause function.
			break;

		case GameState.GameOver:
			PlayersSheepState ();
			_score.GetWinner ();
			_score.enabled = false;
			_newWolf.enabled = false;
			_pause.canPause = false;
			SoundManager.Instance.PlayMusic ("GameOver");
			break;

		case GameState.GamePaused:
			PausePlayersToggle ();
			PauseNPSheepToggle ();
			PauseUIToggle ();
			AudioListener.pause = true;
			break;

		case GameState.GameUnpaused:
			Invoke ("PausePlayersToggle", 0.1f);	//delay to prevent killing if A is pressed on Resume.
			PauseNPSheepToggle ();
			PauseUIToggle ();
			AudioListener.pause = false;
			break;
		}
	}

	void Awake()
	{
		_gameData = GameObject.Find ("GameData").GetComponent<GameData> ();
		GetComponent <TimeManager> ().GameDuration (_gameData._gameDuration);	//assign game duration chosen by the players.
	}

	void Start()
	{
		_score = GetComponent<ScoreManager> ();
		_newWolf = GetComponent<NewWolfManager> ();
		_howlMan = GetComponent<HowlManager> ();

		StartCoroutine (InitGame ());
	}

	IEnumerator InitGame()
	{
		//Are we playtesting in the game scene ?
		#if (UNITY_EDITOR)
		GameObject playtest = GameObject.Find("PlaytestInit(Clone)");
		if (playtest)
		{
			while(!playtest.GetComponent<PlaytestRegistration>().readyToPlaytest)
			{
				yield return null;
			}
		}
		#endif

		//populate the list and spawn the players
		playersInGame = _players.InitialPlayerSpawn (_gameData.registeredPlayers);
		yield return new WaitForEndOfFrame();
		SetGameState (GameState.GameReady);
	}
		
	void PausePlayersToggle()
	{
		for(int i = 0 ; i < playersInGame.Count ; i++)
		{
			playersInGame [i].GetComponent<PlayerData> ().PauseToggle ();
		}
	}

	void PauseNPSheepToggle()
	{
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
		_howlMan.currentlyPlaying = !_howlMan.currentlyPlaying;
	}

	void PlayersSheepState()
	{
		for(int i = 0 ; i < playersInGame.Count ; i++)
		{
			playersInGame [i].GetComponent<PlayerData> ().SetPlayerState (PlayerData.PlayerState.Sheep);
		}
	}

	public void RestartGame()
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
}
