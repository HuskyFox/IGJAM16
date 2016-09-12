using UnityEngine;
using System.Collections;

public class NewWolfManager : UnitySingleton <NewWolfManager>
{	
	//variables for the random switch after a random amount of time without any kill
	public int minTimeBetweenWolfSwitch = 10;
	public int maxTimeBetweenWolfSwitch = 30;
	private int timeBetweenSwitch;
	private bool isRandomTimeSet = false;
	float timer;
	int currentWolfIndex = 0;

	DevicesManager devicesManager;

	void Awake()
	{
		//reference to the DevicesManager script to get access to the list of the players.
		devicesManager = GetComponent<DevicesManager> ();
	}

	void Update()
	{	
		timer += Time.deltaTime;

		//generate a random time if not done yet
		if (!isRandomTimeSet)
			GenerateRandomTimeBetweenSwitch ();

		//when the time is up, calls the function to switch the wolf
		if (timer >= timeBetweenSwitch)
			CreateRandomWolf ();
	}

	int GenerateRandomTimeBetweenSwitch()
	{
		timeBetweenSwitch = Random.Range (minTimeBetweenWolfSwitch, maxTimeBetweenWolfSwitch +1);
		isRandomTimeSet = true;
		//print ("Time before wolf switch:" + timeBetweenSwitch + " secs");
		return timeBetweenSwitch;
	}

	public void CreateRandomWolf()
	{
		timer = 0f;
		isRandomTimeSet = false;
		currentWolfIndex = CreateNewRandomNumber();
		//SoundManager.Instance.PlayRandomFarmerShout();
		//timeManager.gameObject.SetActive(true);
		//timeManager.StartNewWolfCountdown();
		//speechBubble.SetActive (true);
		//Invoke ("MakeSpeechBubbleDisappear", 1f);
		MakeWolf (currentWolfIndex);

	}

	int CreateNewRandomNumber() 
	{
		if (devicesManager.players.Count > 1)
		{
			int randomPlayerIndex = 0;
			do {
				randomPlayerIndex = Random.Range (1, devicesManager.players.Count +1);
			} while(randomPlayerIndex == currentWolfIndex);
			return randomPlayerIndex;
		} else
			return 1;
	}

	void MakeWolf(int wolfIndex)
	{
		for (int i = 0 ; i < devicesManager.players.Count ; i++)
		{
			//we make sure all the players are back to sheep state
			devicesManager.players [i].isWolf = false;
			devicesManager.players[i].tag = "PlayerSheep";

			//and we set the boolean and the tag of the new wolf.
			if(i+1 == wolfIndex)
			{
				devicesManager.players [i].isWolf = true;
				devicesManager.players[i].tag = "Wolf";
				print("Player " + wolfIndex + " is the wolf!");
			}
		}
	}
}
