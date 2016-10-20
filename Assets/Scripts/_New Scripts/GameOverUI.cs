//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using System.Collections.Generic;
//
//public class GameOverUI : MonoBehaviour 
//{
//	public GameObject restartButton;
//	public Text winnerIsText;
//	public Text playerX;
//
//	void OnEnable()
//	{
//		Invoke ("RestartButton", 5.5f);
//	}
//		
//	public void DeclareWinners (List<int> winnersToDeclare)
//	{
//		if (winnersToDeclare.Count > 1)
//			winnerIsText.text = "The winners are..";
//
//		playerX.text = "";
//
//		for (int i = 0 ; i < winnersToDeclare.Count ; i++)
//		{
//			int winner = winnersToDeclare [i];
//			playerX.text += "Player " + winner;
//
//			if (i < winnersToDeclare.Count-1)
//				playerX.text += ", ";
//		}
//	}
//
//	void RestartButton()
//	{
//		restartButton.SetActive (true);
//	}
//}
