using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour 
{
	public GameObject gameOverUI;
	public List <PlayerController> winners = new List<PlayerController>();
	public List<string> winnerIndexes = new List<string>();

	void OnEnable()
	{
		DeclareSeveralWinners (winnerIndexes);
	}

	void DeclareSeveralWinners (List<string> winnersToDeclare)
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
}
