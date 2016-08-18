using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WolfManager : MonoBehaviour 
{
	public int minTimeBetweenWolfSwitch = 10;
	public int maxTimeBetweenWolfSwitch = 30;
	private int timeBetweenSwitch;

	public GameObject playerPrefab;

	public bool isTimeSet = false;

	public int currentWolfIndex;
	public bool isWolfCreated = false;
	const int maxPlayers = 3;
	public bool isWolf { get; set; }

	float timer;

	PlayerManager playerManager;

	void Start()
	{
		playerManager = GameObject.Find ("PlayerManager").GetComponent<PlayerManager> ();
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (!isTimeSet)
			GenerateRandomTimeBetweenSwitch ();

		if (timer >= timeBetweenSwitch)
			CreateRandomWolf ();
	}

	void CreateRandomWolf()
	{
		timer = 0f;
		isTimeSet = false;
		//MakeEveryoneASheep ();
		playerManager.SendMessage("MakeEveryoneASheep");
		currentWolfIndex = CreateNewRandomNumber ();
		if (currentWolfIndex > 0) {
			var wolf = GameObject.Find ("Player_" + currentWolfIndex).GetComponent<Player> ();
			wolf.MakeWolf ();
			isWolfCreated = true;
			Debug.Log ("A new wolf was created.");
		}
	}

	/*void MakeEveryoneASheep() 
	{
		for(int i=1;i<=maxPlayers;i++) {
			GameObject.Find("Player_"+i).GetComponent<Player>().MakeSheep();
		}
	}*/

//	public void MakeWolf() {
//		isWolf = true;
//		tag = "Wolf";
//	}
//
//	public void MakeSheep() {
//		isWolf = false;
//		tag = "Player";
//	}

	int CreateNewRandomNumber() 
	{
		if (playerManager.players.Count > 1) {
			int randomPlayerIndex = 0;
			do {
				randomPlayerIndex = Random.Range (1, playerManager.players.Count +1);
			} while(randomPlayerIndex == currentWolfIndex);

			return randomPlayerIndex;
		} else
			return 1;
	}

	int GenerateRandomTimeBetweenSwitch()
	{
		timeBetweenSwitch = Random.Range (minTimeBetweenWolfSwitch, maxTimeBetweenWolfSwitch);
		Debug.Log (timeBetweenSwitch);
		isTimeSet = true;
		return timeBetweenSwitch;
	}
}
