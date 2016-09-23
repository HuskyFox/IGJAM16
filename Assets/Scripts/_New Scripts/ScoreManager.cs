using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	public int playerKillPoints = 20;
	public int npsheepKillPoints = -20;
	public int killedByPlayerPoints = -20;

	private UIManager scoreUI;
	private List<PlayerController> players = new List <PlayerController> ();
	private bool success = true;

	void OnEnable()
	{
		scoreUI = GameObject.Find ("Canvas").GetComponent<UIManager> ();
		players = GameStateManager.Instance.playersInGame;
		InitializeScore ();
	}

	//setactive the scores depending on the number of players
	void InitializeScore ()
	{
		for (int i = 0 ; i < players.Count ; i ++)
		{
			players [i].scoreKeeper = 0;
			scoreUI.ActivateScore ((i+1), players[i].scoreKeeper);
		}
	}

	//update the scores
	public void SuccessfulKillScoreUpdate (PlayerController killer, PlayerController victim)
	{
		int killerIndex = killer.playerIndex;
		int victimIndex = victim.playerIndex;
		killer.scoreKeeper += playerKillPoints;
		victim.scoreKeeper += killedByPlayerPoints;

		scoreUI.UpdateScore (killerIndex, killer.scoreKeeper, success);
		scoreUI.UpdateScore (victimIndex, victim.scoreKeeper, !success);
	}

	public void UnsuccessfulKillScoreUpdate (PlayerController killer)
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
			int winnerIndex = players [i].playerIndex + 1;
			if (players [i].scoreKeeper == highestScore)
				winnersToDeclare.Add (winnerIndex);
		}
		scoreUI.GameOver (winnersToDeclare);
	}
}
