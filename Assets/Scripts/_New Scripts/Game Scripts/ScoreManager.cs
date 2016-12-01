using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	[SerializeField] private int _playerKillPoints = 20;
	[SerializeField] private int _npsheepKillPoints = -20;
	[SerializeField] private int _killedByPlayerPoints = -20;
	[SerializeField] private ScoreUI _scoreUI;
	[SerializeField] private GameOverUI _gameOverUI;

	private List<PlayerData> _players = new List <PlayerData> ();
	private bool _success = true;

	void OnEnable()
	{
		//gets the list of the registered and spawned players.
		_players = GetComponent<GameStateManager> ().playersInGame;
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

	//update the scores
	public void ScoreUpdate (PlayerData killer, PlayerData victim)
	{
		//update the score of the two players.
		killer.scoreKeeper += _playerKillPoints;
		victim.scoreKeeper += _killedByPlayerPoints;

		//(see ScoreUI script)
		_scoreUI.UpdateScore (killer.playerIndex, killer.scoreKeeper, _success);
		_scoreUI.UpdateScore (victim.playerIndex, victim.scoreKeeper, !_success);
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
}
