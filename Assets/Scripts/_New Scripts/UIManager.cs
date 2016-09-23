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
	public float gameTimeLeft { get; set;}
	float mins;
	float seconds;
	Animation timerAnim;
	bool animPlaying = false;
	public float wolfSeconds { get ; set; }
	public bool showWolfCountdown {get ; set;}


	//GAMEOVER VARIABLES
	public GameObject gameOverUI;
	public GameObject restartButton;
	public Text winnerIsText;
	public Text playerX;


	void Awake()
	{
		timerAnim = timerLabel.GetComponent<Animation> ();
		InitGameUI ();
	}

	public void InitGameUI ()
	{
		GameObject[] showOnGameOver = GameObject.FindGameObjectsWithTag ("GameOverUI");
		for (int i = 0; i < showOnGameOver.Length; i++)
			showOnGameOver [i].SetActive (false);
	}

	//score
	public void ActivateScore(int index, int score)
	{
		if(index <= playerScores.Count)
			playerScores [index - 1].scoreLabel.text = score.ToString ();
		else
		{
			GameObject scoreToActivate = Instantiate <GameObject> (scorePrefab);
			scoreToActivate.transform.SetParent (scoreUI.transform, false);

			playerScores.Add (new Score (scoreToActivate, index, score));
		}
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


	//Game & Wolf Countdown
	void Update()
	{
		mins = Mathf.Floor(gameTimeLeft / 60);
		seconds = Mathf.Floor (gameTimeLeft % 60);
		timerLabel.text = string.Format ("{0:0}:{1:00}", mins, seconds);

		if(gameTimeLeft + 0.25f == 5.25f && gameTimeLeft > 0)
			if(!animPlaying)
				StartCoroutine (TimerAnimation ());
			
		wolfCountdown.text = wolfSeconds.ToString ("F0");
		if (showWolfCountdown) 
			wolfCountdownDisplay.SetActive (true);
		else if (!showWolfCountdown)
			wolfCountdownDisplay.SetActive (false);	
	}

	IEnumerator TimerAnimation()
	{
		animPlaying = true;
		timerAnim.Play ();
		yield return new WaitForSeconds (6f);
		timerAnim.Stop ();
		animPlaying = false;
	}


	//Game Paused



	//Game Over
	public void GameOver (List <int> winners)
	{
		//showWolfCountdown = false;
		StartCoroutine (GameOverUI (winners));
	}

	IEnumerator GameOverUI (List<int> winnersIndex)
	{
		DeclareWinners (winnersIndex);

		yield return new WaitForSeconds (5.5f);

		restartButton.SetActive (true);
	}

	void DeclareWinners (List<int> winnersToDeclare)
	{
		if (winnersToDeclare.Count > 1)
			winnerIsText.text = "The winners are..";

		playerX.text = "";

		for (int i = 0 ; i < winnersToDeclare.Count ; i++)
		{
			int winner = winnersToDeclare [i];
			playerX.text += "Player " + winner;

			if (i < winnersToDeclare.Count-1)
				playerX.text += ", ";
		}
		gameOverUI.SetActive (true);
	}
}
