using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour 
{
	private GameObject gameOverUI;
	private GameObject restartButton;
	public List <PlayerController> winners = new List<PlayerController>();
	public List<string> winnerIndexes = new List<string>();

	public delegate void RestartGame ();
	public static event RestartGame OnRestartGame;

	void OnEnable()
	{
		gameOverUI = GameObject.Find ("Canvas").transform.Find ("GameOver").gameObject;
		restartButton = GameObject.Find ("Canvas").transform.Find ("RestartButton").gameObject;
		StartCoroutine (GameOverUI (winnerIndexes));
	}

	IEnumerator GameOverUI (List<string> winners)
	{
		DeclarelWinners (winners);

		yield return new WaitForSeconds (5.5f);

		EnableRestart ();
	}

	void DeclarelWinners (List<string> winnersToDeclare)
	{
		if (winnersToDeclare.Count > 1)
			gameOverUI.transform.Find ("WinnerImage/WinnerText").gameObject.GetComponent<Text> ().text = "The winners are..";
		
		gameOverUI.transform.Find ("PlayerX/Text").gameObject.GetComponent<Text> ().text = "";

		for (int i = 0 ; i < winnersToDeclare.Count ; i++)
		{
			string winner = winnersToDeclare [i];
			gameOverUI.transform.Find ("PlayerX/Text").gameObject.GetComponent<Text> ().text += "Player " + winner;

			if (i < winnersToDeclare.Count-1)
			{
				gameOverUI.transform.Find ("PlayerX/Text").gameObject.GetComponent<Text> ().text += ", ";
			}
		}

		gameOverUI.SetActive (true);
	}

	void EnableRestart()
	{
		restartButton.SetActive (true);
	}

	void OnDisable()
	{
		if (OnRestartGame != null)
			OnRestartGame ();

		winners.Clear ();
		winnerIndexes.Clear ();
		if(gameOverUI.activeInHierarchy)
			gameOverUI.SetActive (false);
		restartButton.SetActive (false);
	}
}
