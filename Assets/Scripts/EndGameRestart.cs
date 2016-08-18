using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameRestart : MonoBehaviour {

    public Text winningPlayerText;
    public Timer timer;


	public void DeclareWinner ()
    {
        winningPlayerText.text = "Player " + timer.gameWinner;
	}
}
