using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class WolfManager : MonoBehaviour 
{
	public int minTimeBetweenWolfSwitch = 10;
	public int maxTimeBetweenWolfSwitch = 30;
	private int timeBetweenSwitch;
	public Text wolfSwitchWarning;
	bool setWarningText = false;

	public bool isTimeSet = false;

	public int currentWolfIndex;
	[HideInInspector]public bool isWolfCreated = false;
	const int maxPlayers = 3;
	[HideInInspector]public bool isWolf { get; set; }

	float timer;

	PlayerManager playerManager;
    
    void OnEnable ()
    {
        Sheep.OnNpSheepWasKilled += OnNPSheepDied;
    }

    void OnDisable()
    {
        Sheep.OnNpSheepWasKilled -= OnNPSheepDied;
    }

    void OnNPSheepDied(Player killer, Sheep sheep)
    {
        CreateRandomWolf();
    }

	void Start()
	{
		playerManager = GameObject.Find ("PlayerManager").GetComponent<PlayerManager> ();
		wolfSwitchWarning.text = "";
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (!isTimeSet)
			GenerateRandomTimeBetweenSwitch ();

		if (timer >= timeBetweenSwitch) 
			CreateRandomWolf ();

	}

	IEnumerator ShowWolfWarning (string warning, float delay)
	{
		wolfSwitchWarning.text = "A new wolf was chosen!";
		yield return new WaitForSeconds (delay);
		wolfSwitchWarning.text = "";
	}

    void InvokeCreateWolf()
    {
        Invoke("CreateRandomWolf", 5f);
    }

    void CreateRandomWolf()
    {
        timer = 0f;
        isTimeSet = false;
        //MakeEveryoneASheep ();
        playerManager.SendMessage("MakeEveryoneASheep");
        currentWolfIndex = CreateNewRandomNumber();

        if (currentWolfIndex > 0)
        {
            var wolf = GameObject.Find("Player_" + currentWolfIndex).GetComponent<Player>();
            wolf.MakeWolf();
            isWolfCreated = true;
            StartCoroutine(ShowWolfWarning("Abc", 1f));
            Debug.Log("A new wolf was created.");
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
		timeBetweenSwitch = Random.Range (minTimeBetweenWolfSwitch, maxTimeBetweenWolfSwitch +1);
		Debug.Log (timeBetweenSwitch);
		isTimeSet = true;
		return timeBetweenSwitch;
	}
}
