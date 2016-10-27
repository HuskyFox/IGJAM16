using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour 
{
	[SerializeField]
	GameObject UI;
	[SerializeField]
	GameObject restartButton;
	[SerializeField]
	Text winnerIsText;
	[SerializeField]
	Text playerX;

	public void GameOver (List <int> winners)
	{
		DeclareWinners (winners);
		Invoke ("RestartButton", 5.5f);
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

		UI.SetActive (true);
	}

	void RestartButton()
	{
		restartButton.SetActive (true);
		EventSystem.current.SetSelectedGameObject (restartButton);
	}
}
