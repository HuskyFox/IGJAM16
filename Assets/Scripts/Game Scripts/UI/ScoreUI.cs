using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour 
{
	[SerializeField] private GameObject _scorePrefab;
	[SerializeField] private Color _successfulKillColor = Color.green;
	[SerializeField] private Color _unsuccessfulKillColor = Color.red;

	private List<Score> _playerScores  = new List <Score> ();

	public void ActivateScore(int index, int score)
	{
		//Only resets the score if the score is already displayed (if the game was restarted)
		if(index <= _playerScores.Count)
			_playerScores [index - 1].scoreLabel.text = score.ToString ();
		else
		{
			//Instantiates the score display gameobject.
			GameObject scoreToActivate = Instantiate <GameObject> (_scorePrefab);
			scoreToActivate.transform.SetParent (transform, false);

			//Creates a new Score and adds it to the list.
			_playerScores.Add (new Score (scoreToActivate, index, score));
		}
	}

	// This function handles the score update, and the success bool value determines what happens.
	public void UpdateScore (int index, int newScore, bool success)
	{
		//finds the player's score in the list,
		Score scoreToUpdate = _playerScores [index -1];
		//updates its text,
		scoreToUpdate.scoreLabel.text = newScore.ToString ();
		//and starts the right animation depending on if it's a success or a fail.
		if (success)
			StartCoroutine (ScoreAnimation (scoreToUpdate.anim, scoreToUpdate.scoreLabel, _successfulKillColor));
		if(!success)
			StartCoroutine (ScoreAnimation (scoreToUpdate.anim, scoreToUpdate.scoreLabel, _unsuccessfulKillColor));
	}

	//animates the score text for a few seconds. The color depends on the success or fail of the kill.
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
