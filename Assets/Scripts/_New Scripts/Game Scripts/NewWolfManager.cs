using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewWolfManager : MonoBehaviour
{	
	//variables for the random switch after a random amount of time without any kill
	[SerializeField] int minTimeBetweenWolfSwitch = 10;
	[SerializeField] int maxTimeBetweenWolfSwitch = 30;
	[SerializeField] GunAnimation gunAnim;
	[SerializeField] GameObject UI;
	private int timeBetweenSwitch;
	private bool isRandomTimeSet = false;
	private float timer;
	private int currentWolfIndex = 0;
	//private TimeManager timeManager;

	List <PlayerData> players = new List<PlayerData>();

	public delegate void TimeForANewWolf(float countdown);
	public static event TimeForANewWolf OnTimeForANewWolf;

	void OnEnable()
	{
		players = GetComponent<GameStateManager> ().playersInGame;
		//timeManager = GetComponent<TimeManager> ();
		CreateRandomWolf ();
	}

	void Update()
	{	
		//if(timeManager.isGameStarted) 
		//{
			timer += Time.deltaTime;

			//generate a random time if not done yet
			if (!isRandomTimeSet)
				GenerateRandomTimeBetweenSwitch ();

			//when the time is up, calls the function to switch the wolf
			if (timer >= timeBetweenSwitch)
				CreateRandomWolf ();
		//}
	}

	int GenerateRandomTimeBetweenSwitch()
	{
		timeBetweenSwitch = Random.Range (minTimeBetweenWolfSwitch, maxTimeBetweenWolfSwitch +1);
		isRandomTimeSet = true;
		return timeBetweenSwitch;
	}

	public void CreateRandomWolf()
	{
		timer = 0f;
		isRandomTimeSet = false;
		currentWolfIndex = CreateNewRandomNumber();
		//timeManager.timeForANewWolf = true;
		gunAnim.StartAnim ();
		UI.SetActive (true);
		StartCoroutine (MakeWolf ());
		//if (OnTimeForANewWolf != null)
		//	OnTimeForANewWolf (wolfCountdown);
		//else
		//	print ("No listener");
	}

	int CreateNewRandomNumber() 
	{
		if (players.Count > 1)
		{
			int randomPlayerIndex = 0;
			do {
				randomPlayerIndex = Random.Range (1, players.Count +1);
			} while(randomPlayerIndex == currentWolfIndex);
			return randomPlayerIndex;
		} else
			return 1;
	}

	IEnumerator MakeWolf()
	{
		PlayerData nextWolf = null;

		for (int i = 0 ; i < players.Count ; i++)
		{
			PlayerData player = players [i];
			//we make sure all the players are back to sheep state
			//player.tag = "PlayerSheep";

			//and we set the boolean and the tag of the new wolf.
			if (i + 1 == currentWolfIndex)
				nextWolf = player;
		}

		//yield return new WaitForSeconds (wolfCountdown + 0.15f);
		yield return new WaitUntil (() => UI.activeSelf == false);

		nextWolf.SetPlayerState (PlayerData.PlayerState.Wolf);
		//nextWolf.isWolf = true;
		//nextWolf.tag = "Wolf";
		print(nextWolf.name + " is the wolf!");
	}

	void OnDisable()
	{
		StopCoroutine ("MakeWolf");
		if (UI.activeSelf)
			UI.SetActive (false);
		if(gunAnim != null)
			gunAnim.HideForGameOver ();
		
		//UI.SetActive (false);
	}
}
