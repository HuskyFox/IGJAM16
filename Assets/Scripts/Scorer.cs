using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scorer : MonoBehaviour {

    public int noOfPlayers;

    public GameObject playerOneScore;
    int oneScore;

    public GameObject playerTwoScore;
    int twoScore;

    public GameObject playerThreeScore;
    int threeScore;

    public GameObject playerFourScore;
    int fourScore;

    public bool currentlyPlaying;

	// Use this for initialization
	void Start () {

        if (noOfPlayers == 2)
        {
            playerThreeScore.gameObject.SetActive(false);
            playerFourScore.gameObject.SetActive(false);
        }

        if (noOfPlayers == 3)
        {
            playerFourScore.gameObject.SetActive(false);
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	if (currentlyPlaying)
        {
            playerOneScore.GetComponent<Text>().text = oneScore.ToString();
            playerTwoScore.GetComponent<Text>().text = twoScore.ToString();
            playerThreeScore.GetComponent<Text>().text = threeScore.ToString();
            playerFourScore.GetComponent<Text>().text = fourScore.ToString();

        }
	}
}
