using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	[SerializeField] private int _playerKillPoints = 20;
	[SerializeField] private int _npsheepKillPoints = -20;
	[SerializeField] private int _killedByPlayerPoints = -5;
	[SerializeField] private ScoreUI _scoreUI;
	[SerializeField] private GameOverUI _gameOverUI;

	private List<PlayerData> _players = new List <PlayerData> ();
	private bool _success = true;
	private int _killPoints = 0;

	void OnEnable()
	{
		//gets the list of the registered and spawned players.
		_players = GetComponent<GameStateManager> ().playersInGame;

		//initialize the successful kill amount of points.
		_killPoints = _playerKillPoints;

		PlayerActions.OnWolfHowled += SetKillPoints;
	}

	//setactive the scores depending on the number of players
	public void InitializeScore ()
	{
		for (int i = 0 ; i < _players.Count ; i ++)
		{
			//resets the score to 0.
			_players [i].scoreKeeper = 0;
			//(see ScoreUI script)
			_scoreUI.ActivateScore (_players[i].playerIndex, _players[i].scoreKeeper);
		}
	}

	//decrease the amount of points a player gets for a successful kill, if he has used the howl
	void SetKillPoints(PlayerActions notUsed)
	{
		//only the first two howls decrease the amount of points : 20 -> 10 -> 5.
		if(_killPoints > 5)
			_killPoints /= 2;
	}

	//update the scores
	//Two overload methods, depending on if a player or a NPSheep was killed.
	public void ScoreUpdate (PlayerData killer, PlayerData victim)
	{
		//update the score of the two players.
		killer.scoreKeeper += _killPoints;
		victim.scoreKeeper += _killedByPlayerPoints;

		//(see ScoreUI script)
		_scoreUI.UpdateScore (killer.playerIndex, killer.scoreKeeper, _success);
		_scoreUI.UpdateScore (victim.playerIndex, victim.scoreKeeper, !_success);

		//Reset the amount of points of a successful kill.
		_killPoints = _playerKillPoints;
	}

	public void ScoreUpdate (PlayerData killer)
	{
		//update the score of the killer
		killer.scoreKeeper += _npsheepKillPoints;

		//(see ScoreUI script)
		_scoreUI.UpdateScore (killer.playerIndex, killer.scoreKeeper, !_success);
	}

	//release the highest score
	public void GetWinner ()
	{
		//creates and populates a list with the final scores of all the players.
		List <int> finalScores = new List<int> ();
		for (int i = 0; i < _players.Count; i++)
		{
			int score = _players[i].scoreKeeper;
			finalScores.Add (score);
		}

		//Finds the highest score in the list.
		int highestScore = Mathf.Max (finalScores.ToArray ());

		//creates and populates a list with the index of the player(s) whose score is equal to the highest score.
		List <int> winnersToDeclare = new List<int> ();
		for (int i = 0; i < _players.Count; i++)
		{
			int winnerIndex = _players [i].playerIndex;
			if (_players [i].scoreKeeper == highestScore)
				winnersToDeclare.Add (winnerIndex);
		}

		//Pass in the list of winners to the game over UI script.
		_gameOverUI.GameOver (winnersToDeclare);
	}

	void OnDisable()
	{
		PlayerActions.OnWolfHowled -= SetKillPoints;
	}
}
