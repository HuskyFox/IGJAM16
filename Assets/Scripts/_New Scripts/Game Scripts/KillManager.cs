using UnityEngine;
using System.Collections;

/* This script is in charge of what happens when there is a kill.
 * It receives the identity of the killer and the victim (player or NPsheep) from the event broadcast by the killer.
 * Depending on the event (and its parameters), the Kill function called is different (Overload methods). */
public class KillManager : MonoBehaviour
{
	[SerializeField] private PlayerSpawnerManager _spawnManager;
	[SerializeField] private iTweenEvent _camShake;
	private ScoreManager _scoreManager;
	private NewWolfManager _wolfManager;
	private KillFeedback _killerFeedback = null;	//assigned after each kill
	private KillFeedback _victimFeedback = null;	//assigned after each kill
	private PlayerData _killerData = null;			//assigned after each kill
	private PlayerData _victimData = null;			//assigned after each kill

	void Awake()
	{
		_scoreManager = GetComponent<ScoreManager> ();
		_wolfManager = GetComponent<NewWolfManager> ();
	}

	void OnEnable()
	{
		PlayerActions.OnPlayerWasKilled += Kill;
		PlayerActions.OnNPSheepWasKilled += Kill;
	}

	//Success kill
	void Kill(GameObject killer, GameObject victim)
	{
		//Assign values to the scripts.
		GetReferences (killer, victim);

		//Set the killer back to sheep state.
		_killerData.SetPlayerState (PlayerData.PlayerState.Sheep);

		StartCoroutine (SuccessKill ());
	}

	void GetReferences(GameObject killer, GameObject victim)
	{
		_killerFeedback = killer.GetComponent<KillFeedback> ();
		_killerData = killer.GetComponent<PlayerData> ();

		_victimFeedback = victim.GetComponent<KillFeedback> ();
		_victimData = victim.GetComponent<PlayerData> ();
	}

	IEnumerator SuccessKill ()
	{
		//Pause the music and play the heartbeats.
		SoundManager.Instance.HeartBeatOn ();

		StartCoroutine (_victimFeedback.VictimFeedback (_killerFeedback));				//See KillFeedback script.

		//Wait for the feedback coroutine to finish...
		yield return StartCoroutine (_killerFeedback.KillerFeedback (_victimFeedback));	//See KillFeedback script.

		_spawnManager.RespawnPlayer (_victimData);

		SoundManager.Instance.HeartBeatOff ();

		_scoreManager.ScoreUpdate (_killerData, _victimData);

		_wolfManager.CreateRandomWolf ();
	}
		
	//Fail kill
	void Kill(GameObject killer, NPSheep victim)
	{
		//Assign values to the scripts.
		GetReferences (killer);

		//Set the killer back to sheep state.
		_killerData.SetPlayerState (PlayerData.PlayerState.Sheep);

		StartCoroutine(victim.TakeDamage ());	//see NPSheep script.
		StartCoroutine(_killerFeedback.KillerFeedback ());	//see KillFeedback script.

		if (_camShake) 
			_camShake.Play();	//iTweenEvent.

		_scoreManager.ScoreUpdate (_killerData);
		_wolfManager.CreateRandomWolf ();
	}

	void GetReferences(GameObject killer)
	{
		_killerFeedback = killer.GetComponent<KillFeedback> ();
		_killerData = killer.GetComponent<PlayerData> ();
	}

	void OnDisable()
	{
		PlayerActions.OnPlayerWasKilled -= Kill;
		PlayerActions.OnNPSheepWasKilled -= Kill;
	}
}
