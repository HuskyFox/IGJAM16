using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Score
{
	public GameObject scoreDisplay;
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