using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/* This script handles the activation of the GameOver-related UI.
 * Its main function is called by the GameStateManager script.*/
public class GameOverUI : MonoBehaviour 
{
	[SerializeField] private GameObject _UI;
	[SerializeField] private GameObject _restartButton;
	[SerializeField] private Text _winnerIsText;
	[SerializeField] private Text _playerX;

	public void GameOver (List <int> winners)
	{
		DeclareWinners (winners);
		Invoke ("RestartButton", 5.5f);		//set active the restart button after 5.5 seconds for synchronisation with animations
	}

	void DeclareWinners (List<int> winnersToDeclare)
	{
		//Change the text if there is a tie.
		if (winnersToDeclare.Count > 1)
			_winnerIsText.text = "The winners are..";

		_playerX.text = "";

		for (int i = 0 ; i < winnersToDeclare.Count ; i++)
		{
			int winner = winnersToDeclare [i];
			_playerX.text += "Player " + winner;

			//add a "," between the winners if there are several.
			if (i < winnersToDeclare.Count-1)
				_playerX.text += ", ";
		}

		_UI.SetActive (true);
	}

	void RestartButton()
	{
		_restartButton.SetActive (true);
		EventSystem.current.SetSelectedGameObject (_restartButton);
	}
}
