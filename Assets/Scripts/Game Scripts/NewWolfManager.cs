using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This script is in charge of creating the wolf,
 * either randomly when no kill happens during a certain amount of time (that can be defined),
 * or automatically when a kill happens.*/
public class NewWolfManager : MonoBehaviour
{	
	[HideInInspector] public bool currentlyPlaying = false;	
	[SerializeField] private bool _negativeFeedbackLoop = false;
	[SerializeField] private int _minTimeBetweenWolfSwitch = 20;	//for random switch
	[SerializeField] private int _maxTimeBetweenWolfSwitch = 30;	//for random switch
	[SerializeField] private GunAnimation _gunAnim;
	[SerializeField] private GameObject _UI;
	[SerializeField] private HowlCoolDownUI _howlUI;
	private int _timeBetweenSwitch;				//for random switch
	private bool _isRandomTimeSet = false;		//for random switch
	private float _timer;						//for random switch
	private int _wolfIndex = 0;
	private List <PlayerData> _players = new List<PlayerData>();
	private List<int> _wolfCounters = new List<int>();		//list for the number of times each player has been the wolf.

	void OnEnable()
	{
		//get the list of the players in game and create the first wolf.
		PopulateLists ();
		currentlyPlaying = true;
		CreateRandomWolf ();
	}
		
	void PopulateLists()
	{
		_players = GetComponent<GameStateManager> ().playersInGame;

		if(_negativeFeedbackLoop)
		{
			//add an index to the list with an initial value of 0.
			for (int i = 0; i < _players.Count; i++)
			{
				_wolfCounters.Add (0);
			}
		}
	}

	void Update()
	{	
		if (currentlyPlaying)
			_timer += Time.deltaTime;

		//generate a random time if not done yet
		if (!_isRandomTimeSet)
			GenerateRandomTimeBetweenSwitch ();

		//when the time is up, calls the function to switch the wolf
		if (_timer >= _timeBetweenSwitch)
			CreateRandomWolf ();
	}

	int GenerateRandomTimeBetweenSwitch()
	{
		_timeBetweenSwitch = Random.Range (_minTimeBetweenWolfSwitch, _maxTimeBetweenWolfSwitch +1);
		_isRandomTimeSet = true;
		return _timeBetweenSwitch;
	}

	public void CreateRandomWolf()
	{
		//reset the timer of the random switch
		_timer = 0f;
		_isRandomTimeSet = false;

		//Reset the howl cooldown UI.
		_howlUI.ResetCooldown ();

		//get a random number that will be the next wolf's player index
		_wolfIndex = GetNextWolfIndex();

		StartCoroutine (CreateWolf ());
	}

	int GetNextWolfIndex() 
	{
		if(_players.Count > 1)
		{
			int randomPlayerIndex = 0;
			do {
				randomPlayerIndex = Random.Range (1, _players.Count + 1);
			} while(randomPlayerIndex == _wolfIndex);	//a player cannot be the wolf twice in a raw.

			if (_negativeFeedbackLoop)
			{
				//check the counters
				int minCounter = Mathf.Min (_wolfCounters.ToArray ());
				int maxCounter = Mathf.Max (_wolfCounters.ToArray ());

				//if there are more than two players and they have not been the wolf the same amount of times
				if (minCounter != maxCounter && _players.Count >= 3)
				{
					//check the counter of the player that was randomly chosen, and apply a probability to re-do the random choice
					int checkedCounter = _wolfCounters [randomPlayerIndex-1];
					if (checkedCounter == maxCounter)
					{
						if (Random.value <= 0.80f) 		//high probability of doing the random choice again
						{	
							int newRandomIndex = 0;
							do {
								newRandomIndex = Random.Range (1, _players.Count + 1);
							} while(newRandomIndex == _wolfIndex || newRandomIndex == randomPlayerIndex);
							_wolfCounters [newRandomIndex-1] += 1;	//increment the counter of the chosen player.
							return newRandomIndex;
						}
					} 
					else if (minCounter < checkedCounter && checkedCounter < maxCounter)
					{	
						if (Random.value <= 0.40f)		//medium probability of doing the random choice again
						{
							int newRandomIndex = 0;
							do {
								newRandomIndex = Random.Range (1, _players.Count + 1);
							} while(newRandomIndex == _wolfIndex || newRandomIndex == randomPlayerIndex);
							_wolfCounters [newRandomIndex-1] += 1;	//increment the counter of the chosen player.
							return newRandomIndex;
						}
					} 
				} 
			}
			_wolfCounters [randomPlayerIndex-1] += 1;	//increment the counter of the chosen player.
			return randomPlayerIndex;
		} else
			return 1;
	}

	IEnumerator CreateWolf()
	{
		//Start the animation of the gun (see GunAnimation script).
		_gunAnim.StartAnim ();

		//Enable the countdown display
		_UI.SetActive (true);

		//finds the PlayerData script attached to the player whose index was chosen.
		PlayerData nextWolf = null;
		for (int i = 0 ; i < _players.Count ; i++)
		{
			PlayerData player = _players [i];
			player.SetPlayerState (PlayerData.PlayerState.Sheep);	//needed if the function is called after the random time.
			if (i + 1 == _wolfIndex)
				nextWolf = player;
		}

		//wait until the countdown is over...
		yield return new WaitUntil (() => _UI.activeSelf == false);

		//and set the PlayerState to wolf.
		nextWolf.SetPlayerState (PlayerData.PlayerState.Wolf);
		print(nextWolf.name + " is the wolf!");
	}

	void OnDisable()
	{
		StopCoroutine ("MakeWolf");
		if(_UI != null && _UI.activeInHierarchy)
			_UI.SetActive (false);
		if(_gunAnim != null)
			_gunAnim.HideForGameOver ();
	}
}
