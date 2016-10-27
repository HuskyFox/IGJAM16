using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	public int playerKillPoints = 20;
	public int npsheepKillPoints = -20;
	public int killedByPlayerPoints = -20;

	[SerializeField]
	ScoreUI scoreUI;
	[SerializeField]
	GameOverUI gameOverUI;

	List<PlayerData> players = new List <PlayerData> ();
	private bool success = true;

	void OnEnable()
	{
		players = GetComponent<GameStateManager> ().playersInGame;
	}

	//setactive the scores depending on the number of players
	public void InitializeScore ()
	{
		for (int i = 0 ; i < players.Count ; i ++)
		{
			players [i].scoreKeeper = 0;
			scoreUI.ActivateScore (players[i].playerIndex, players[i].scoreKeeper);
		}
	}

	//update the scores
	public void SuccessfulKillScoreUpdate (PlayerData killer, PlayerData victim)
	{
		int killerIndex = killer.playerIndex;
		int victimIndex = victim.playerIndex;
		killer.scoreKeeper += playerKillPoints;
		victim.scoreKeeper += killedByPlayerPoints;

		scoreUI.UpdateScore (killerIndex, killer.scoreKeeper, success);
		scoreUI.UpdateScore (victimIndex, victim.scoreKeeper, !success);
	}

	public void UnsuccessfulKillScoreUpdate (PlayerData killer)
	{
		int killerIndex = killer.playerIndex;
		killer.scoreKeeper += npsheepKillPoints;

		scoreUI.UpdateScore (killerIndex, killer.scoreKeeper, !success);
	}

	//release the highest score
	public void GetWinner ()
	{
		List <int> finalScores = new List<int> ();
		for (int i = 0; i < players.Count; i++)
		{
			int score = players[i].scoreKeeper;
			finalScores.Add (score);
		}

		int highestScore = Mathf.Max (finalScores.ToArray ());

		List <int> winnersToDeclare = new List<int> ();
		for (int i = 0; i < players.Count; i++)
		{
			int winnerIndex = players [i].playerIndex;
			if (players [i].scoreKeeper == highestScore)
				winnersToDeclare.Add (winnerIndex);
		}
		gameOverUI.GameOver (winnersToDeclare);
	}
}
