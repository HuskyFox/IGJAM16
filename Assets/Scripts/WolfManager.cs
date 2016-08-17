using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WolfManager : MonoBehaviour 
{
	public float minTimeBetweenWolfSwitch = 10f;
	public float maxTimeBetweenWolfSwitch = 30f;
	private float timeBetweenSwitch;

	public GameObject playerPrefab;

	public bool isTimeSet = false;

	public int currentWolfIndex;
	public bool isWolfCreated = false;
	const int maxPlayers = 4;
	public bool isWolf { get; set; }

	float timer;

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
		GameObject.Find ("PlayerManager").SendMessage("MakeEveryoneASheep");
		currentWolfIndex = CreateNewRandomNumber ();
		var wolf = GameObject.Find("Player_"+currentWolfIndex).GetComponent<Player>();
		wolf.MakeWolf ();
		isWolfCreated = true;
		Debug.Log("A new wolf was created.");
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
		int randomPlayerIndex = 0;
		do {
			randomPlayerIndex = Random.Range (1, 4);
		} while( randomPlayerIndex==currentWolfIndex );

		return randomPlayerIndex;
	}

	float GenerateRandomTimeBetweenSwitch()
	{
		timeBetweenSwitch = Random.Range (minTimeBetweenWolfSwitch, maxTimeBetweenWolfSwitch);
		Debug.Log (timeBetweenSwitch);
		isTimeSet = true;
		return timeBetweenSwitch;
	}
}
