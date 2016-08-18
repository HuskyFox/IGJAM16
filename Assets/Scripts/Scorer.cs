using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scorer : MonoBehaviour {

    public Text playerOneScore;
    int oneScore;

    public Text playerTwoScore;
    int twoScore;

    public Text playerThreeScore;
    int threeScore;

    public Text playerFourScore;
    int fourScore;

    public bool currentlyPlaying;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (currentlyPlaying)
        {
            playerOneScore.text = oneScore.ToString();
            playerTwoScore.text = twoScore.ToString();
            playerThreeScore.text = threeScore.ToString();
            playerFourScore.text = fourScore.ToString();

        }
	}
}
