using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This script is in charge of creating the wolf,
 * either randomly when no kill happens during a certain amount of time (that can be defined),
 * or automatically when a kill happens.*/
public class NewWolfManager : MonoBehaviour
{	
	[HideInInspector] public bool currentlyPlaying = false;	
	[SerializeField] private int _minTimeBetweenWolfSwitch = 20;	//for random switch
	[SerializeField] private int _maxTimeBetweenWolfSwitch = 30;	//for random switch
	[SerializeField] private GunAnimation _gunAnim;
	[SerializeField] private GameObject _UI;
	[SerializeField] private HowlCoolDownUI _howlUI;
	private int _timeBetweenSwitch;				//for random switch
	private bool _isRandomTimeSet = false;		//for random switch
	private float _timer;						//for random switch
	private int _currentWolfIndex = 0;
	private List <PlayerData> _players = new List<PlayerData>();

	void OnEnable()
	{
		//get the list of the players in game and create the first wolf.
		_players = GetComponent<GameStateManager> ().playersInGame;
		currentlyPlaying = true;
		CreateRandomWolf ();
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
		_currentWolfIndex = CreateNewRandomNumber();

		StartCoroutine (CreateWolf ());
	}

	int CreateNewRandomNumber() 
	{
		if (_players.Count > 1)
		{
			int randomPlayerIndex = 0;
			//a player cannot be the wolf twice in a raw.
			do {
				randomPlayerIndex = Random.Range (1, _players.Count +1);
			} while(randomPlayerIndex == _currentWolfIndex);
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
			_players [i].SetPlayerState (PlayerData.PlayerState.Sheep);	//if the function is called after the random time.
			if (i + 1 == _currentWolfIndex)
				nextWolf = _players[i];
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
