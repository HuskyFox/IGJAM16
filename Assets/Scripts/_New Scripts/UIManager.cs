using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Score
{
	GameObject scoreDisplay;
	public Text scoreLabel;
	public Animation anim;

	Dictionary <int, string> scoreTexts = new Dictionary <int, string>();

	public Score (GameObject newScoreDisplay, int index, int score)
	{
		scoreTexts.Add (1, "One");
		scoreTexts.Add (2, "Two");
		scoreTexts.Add (3, "Three");
		scoreTexts.Add (4, "Four");

		scoreDisplay = newScoreDisplay;
		newScoreDisplay.name = "ScorePlayer_" + index;

		Text indexLabel = scoreDisplay.GetComponent<Text> ();
		indexLabel.text = "Player " + scoreTexts [index];

		scoreLabel = scoreDisplay.transform.Find ("Score").GetComponent<Text> ();
		scoreLabel.text = score.ToString ();

		anim = newScoreDisplay.GetComponent<Animation> ();
	}
}

public class UIManager : MonoBehaviour 
{
	//SCORE VARIABLES
	public GameObject scorePrefab;
	public GameObject scoreUI;
	public Color successfulKillColor = Color.green;
	public Color unsuccessfulKillColor = Color.red;

	List<Score> playerScores  = new List <Score> ();


	//TIME VARIABLES
	public Text timerLabel;
	public GameObject wolfCountdownDisplay;
	public Text wolfCountdown;

	//score
	public void ActivateScore(int index, int score)
	{
		GameObject scoreToActivate = Instantiate <GameObject> (scorePrefab);
		scoreToActivate.transform.SetParent (scoreUI.transform, false);

		playerScores.Add (new Score(scoreToActivate, index, score));
	}

	public void UpdateScore (int index, int newScore, bool success)
	{
		Score scoreToUpdate = playerScores [index];
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

	//WARNING: NEED TO CLEAR THE PLAYERSCORES LIST WHEN BACK TO MENU !!


	//Game Countdown
	public void TimeRemaining(float mins, float seconds)
	{
		timerLabel.text = string.Format ("{0:0}:{1:00}", mins, seconds);
	}

	//Wolf Countdown
	public void WolfCountDown(float seconds, bool active)
	{
		wolfCountdown.text = seconds.ToString ();

		if (active) 
			wolfCountdownDisplay.SetActive (true);
		else if (!active)
			wolfCountdownDisplay.SetActive (false);
	}

	//Game Paused



	//Game Over



}
