using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreManagerSimulator : MonoBehaviour {

	public int wolfPointsPerPlayerKill = 10;
	public int wolfPointsPerNPCKill = -20;
	public int sheepPointsPerPlayerKill = -5;
	public int sheepPointsPerNPCKill = 5;

	public Text[] scoreTexts = new Text[4];

//	public int[] PlayerScores = { scorePlayer1, scorePlayer2, scorePlayer3, scorePlayer4 };

	int[] PlayerScores = new int[4];

//	int scorePlayer1;
//	int scorePlayer2;
//	int scorePlayer3;
//	int scorePlayer4;

	int randomWolfIndex;

//	bool isWolf;
//	bool isSheep;
	bool isTimeSet = false;
	float timer;
	public int minTimeBetweenWolfSwitch = 10;
	public int maxTimeBetweenWolfSwitch = 30;
	private int timeBetweenSwitch;

	void Start()
	{
		for (int i = 0; i <= PlayerScores.Length; i++) 
		{
			PlayerScores [i] = 0;
			print ("qsd");
		}
	}

	void Awake()
	{
		CreateRandomWolf ();
	}
		
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			CreateRandomWolf ();

		timer += Time.deltaTime;
		if (!isTimeSet)
			GenerateRandomTimeBetweenSwitch ();

		if (timer >= timeBetweenSwitch) 
			CreateRandomWolf ();

		SetScore ();

		for (int i = 0; i <= scoreTexts.Length; i++)
		{
			scoreTexts [i].text = "Score: " + PlayerScores[i];
		}
	}

	public void CreateRandomWolf()
	{
		timer = 0f;
		isTimeSet = false;
		MakeEveryoneASheep ();
		randomWolfIndex = Random.Range (0, 4);
		scoreTexts [randomWolfIndex].color = Color.red;
	}

	int GenerateRandomTimeBetweenSwitch()
	{
		timeBetweenSwitch = Random.Range (minTimeBetweenWolfSwitch, maxTimeBetweenWolfSwitch +1);
		Debug.Log (timeBetweenSwitch);
		isTimeSet = true;
		return timeBetweenSwitch;
	}

	void MakeEveryoneASheep()
	{
		scoreTexts [1].color = Color.white;
		scoreTexts [2].color = Color.white;
		scoreTexts [3].color = Color.white;
		scoreTexts [0].color = Color.white;
	}

	void SetScore()
	{
		//the wolf kills a player
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			for (int i = 0; i <= PlayerScores.Length; i++) 
			{
				if (i == randomWolfIndex)
				{
					print (randomWolfIndex);
					PlayerScores [randomWolfIndex] += wolfPointsPerPlayerKill;
					scoreTexts [randomWolfIndex].text = "Score:" + PlayerScores [randomWolfIndex];
				} 
				else 
				{
					PlayerScores [i] += sheepPointsPerPlayerKill;
					scoreTexts [i].text = "Score: " + PlayerScores [i];	
				}
			}
		}

		//the wolf kills a NPsheep
		if (Input.GetKeyDown (KeyCode.T)) 
		{
			for (int i = 0; i <= PlayerScores.Length; i++) 
			{
				if (i == randomWolfIndex)
				{
					print (randomWolfIndex);
					PlayerScores [randomWolfIndex] += wolfPointsPerNPCKill;
					scoreTexts [randomWolfIndex].text = "Score:" + PlayerScores [randomWolfIndex];
				} 
				else 
				{
					PlayerScores [i] += sheepPointsPerNPCKill;
					scoreTexts [i].text = "Score: " + PlayerScores [i];	
				}
			}
		}
	}
}
