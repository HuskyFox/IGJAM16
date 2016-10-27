using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour 
{
	[SerializeField]
	GameObject scorePrefab;
	[SerializeField]
	GameObject scoresDisplay;
	[SerializeField]
	Color successfulKillColor = Color.green;
	[SerializeField]
	Color unsuccessfulKillColor = Color.red;

	List<Score> playerScores  = new List <Score> ();

	public void ActivateScore(int index, int score)
	{
		if(index <= playerScores.Count)
			playerScores [index - 1].scoreLabel.text = score.ToString ();
		else
		{
			GameObject scoreToActivate = Instantiate <GameObject> (scorePrefab);
			scoreToActivate.transform.SetParent (scoresDisplay.transform, false);

			playerScores.Add (new Score (scoreToActivate, index, score));
		}
	}

	public void UpdateScore (int index, int newScore, bool success)
	{
		Score scoreToUpdate = playerScores [index -1];
		scoreToUpdate.scoreLabel.text = newScore.ToString ();
		if (success)
			StartCoroutine (ScoreAnimation (scoreToUpdate.anim, scoreToUpdate.scoreLabel, successfulKillColor));
		if(!success)
			StartCoroutine (ScoreAnimation (scoreToUpdate.anim, scoreToUpdate.scoreLabel, unsuccessfulKillColor));
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
}
