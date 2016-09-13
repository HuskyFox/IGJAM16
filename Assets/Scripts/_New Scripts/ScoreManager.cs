using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	public int playerKillPoints = 20;
	public int npsheepKillPoints = -20;
	public int killedByPlayerPoints = -20;

	public GameObject playerScores;

	public Color successfulKillColor = Color.green;
	public Color unsuccessfulKillColor = Color.red;

	//private List <int> finalScores = new List<int> ();
	//private int highestScore = 0;

	//setactive the scores depending on the number of players
	public void InitializeScore (List <PlayerController> registeredPlayers)
	{
		for (int i = 0 ; i < registeredPlayers.Count ; i ++)
		{
			GameObject playerScore = playerScores.transform.Find("ScorePlayer_" + (i+1)).gameObject;
			playerScore.SetActive (true);

			//Resets the scores.
			Text playerScoreText = playerScore.transform.Find ("Score").GetComponent<Text> ();
			playerScoreText.text = "0";
		}
	}

	//update the scores
	public void SuccessfulKillScoreUpdate (PlayerController killer, PlayerController victim)
	{
		UpdateKillerScore (killer, playerKillPoints, successfulKillColor);
		UpdateVictimScore (victim, killedByPlayerPoints, unsuccessfulKillColor);
	}

	public void UnsuccessfulKillScoreUpdate (PlayerController killer)
	{
		UpdateKillerScore (killer, npsheepKillPoints, unsuccessfulKillColor);
	}

	void UpdateKillerScore (PlayerController killer, int score, Color color)
	{
		Text killerScoreText = killer.score.transform.Find ("Score").GetComponent<Text>();
		int killerScore = int.Parse (killerScoreText.text);

		killerScore += score;
		killerScoreText.text = killerScore.ToString ();

		Animation scoreAnim = killer.score.GetComponent<Animation> ();
		StartCoroutine (ScoreAnimation (scoreAnim, killerScoreText, color));

		killer.scoreKeeper = killerScore;
	}

	void UpdateVictimScore (PlayerController victim, int score, Color color)
	{
		Text victimScoreText = victim.score.transform.Find ("Score").GetComponent<Text>();
		int victimScore = int.Parse (victimScoreText.text);

		victimScore += score;
		victimScoreText.text = victimScore.ToString ();

		Animation scoreAnim = victim.score.GetComponent<Animation> ();
		StartCoroutine (ScoreAnimation (scoreAnim, victimScoreText, color));

		victim.scoreKeeper = victimScore;
	}

	IEnumerator ScoreAnimation (Animation anim, Text score, Color color)
	{
		score.color = color;
		anim.Play ();
		while (anim.isPlaying)
		{
			yield return null;
		}
		score.color = Color.white;
	}

	//release the highest score
	public void GetWinner (List <PlayerController> players)
	{
//		List <int> finalScores = new List<int> ();
//		for (int i = 0; i < players.Count; i++)
//		{
//			PlayerController player = players [i];
//			int score = player.scoreKeeper;
//			finalScores.Add (score);
//			int highestScore = Mathf.Max (finalScores.ToArray ());
//
//			if (player.scoreKeeper == highestScore)
//			{
//				FindObjectOfType<GameOverManager> ().winners.Add (player);
//				print ("The winner is Player " + (i+1) + " with a score of " + highestScore + " !");
//			}
//		}

		List <int> finalScores = new List<int> ();
		for (int i = 0; i < players.Count; i++)
		{
			PlayerController player = players [i];
			int score = player.scoreKeeper;
			finalScores.Add (score);
		}

		int highestScore = Mathf.Max (finalScores.ToArray ());

		for (int i = 0; i < players.Count; i++)
		{
			PlayerController player = players [i];
			if (player.scoreKeeper == highestScore)
			{
				FindObjectOfType<GameOverManager> ().winners.Add (player);
				string winnerIndex = player.name.Replace ("Player_", "");
				FindObjectOfType<GameOverManager> ().winnerIndexes.Add (winnerIndex);
				print ("The winner is Player " + (i+1) + " with a score of " + highestScore + " !");
			}
		}
	}
}
