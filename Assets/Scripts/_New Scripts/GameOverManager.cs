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
		//winnerIndex = winner.name.Replace ("Player_", "");
		if (winnerIndexes.Count == 1)
			DeclareOneWinner (winnerIndexes);
		else
			DeclareSeveralWinners (winnerIndexes);
	}

	void DeclareOneWinner (List<string> winnersToDeclare)
	{
		string winner = winnersToDeclare [0];
		gameOverUI.transform.Find("PlayerX/Text").gameObject.GetComponent<Text>().text = "Player " + winner;
		gameOverUI.SetActive (true);
	}

	void DeclareSeveralWinners (List<string> winnersToDeclare)
	{
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
		//print (winText);
		gameOverUI.SetActive (true);
		//winText = "Player " + winnersToDeclare [0] + ", Player " + winnersToDeclare [1];
	}
}
