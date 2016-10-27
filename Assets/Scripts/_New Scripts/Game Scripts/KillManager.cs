using UnityEngine;
using System.Collections;

public class KillManager : MonoBehaviour
{
	public float timeSpentInWolfShape = 0.3f;
	[SerializeField]
	ScoreManager scoreManager;
	[SerializeField]
	NewWolfManager wolfManager;
	[SerializeField]
	PlayerSpawnerManager spawnManager;

	KillFeedback killerFeedback = null;
	//KillFeedback victimFeedback = null;
	PlayerData killerData = null;
	PlayerData victimData = null;
	//PlayerMovement killerMove;
	PlayerMovement victimMove = null;
	ControllerVibration victimVib = null;
	//PlayerActions killerActions;
	//PlayerActions victimActions;
	AudioSource killerAudio = null;
	AudioSource victimAudio = null;

	void OnEnable()
	{
		PlayerActions.OnPlayerWasKilled += successfulKillMethod;
		PlayerActions.OnNPSheepWasKilled += unsuccessfulKillMethod;
	}

//	void successfulKillMethod (PlayerController killer, PlayerController victim)
//	{
//		killer.isWolf = false;
//
//		StartCoroutine (SuccessKill (killer, victim, timeSpentInWolfShape));
//	}

//	IEnumerator SuccessKill (PlayerController killer, PlayerController victim, float delay)
//	{
//		SoundManager.Instance.PauseMusic ();
//		killer.GetComponent<KillFeedback> ().VictimVibration (victim, delay);
//		killer.GetComponent<KillFeedback> ().ShapeShiftFeedback (killer, delay);
//		killer.GetComponent<KillFeedback> ().SlowMoFeedback (killer, victim, delay);
//		SoundManager.Instance.PlaySuccessKillSound (killer, victim, delay);
//
//		yield return new WaitForSeconds (delay);
//
//		SoundManager.Instance.UnPauseMusic ();
//		scoreManager.SuccessfulKillScoreUpdate (killer, victim);
//		wolfManager.CreateRandomWolf ();
//	}

//	void unsuccessfulKillMethod (PlayerController killer, NPSheep victim)
//	{
//		killer.isWolf = false;
//
//		victim.TakeDamage (victim);
//		victim.CamShake ();
//		killer.GetComponent<KillFeedback> ().ShapeShiftFeedback (killer, timeSpentInWolfShape);
//		SoundManager.Instance.PlayFailKillSound (killer);
//		wolfManager.CreateRandomWolf ();
//
//		scoreManager.UnsuccessfulKillScoreUpdate (killer);
//	}

	void GetReferences(GameObject killer, GameObject victim)
	{
		killerFeedback = killer.GetComponent<KillFeedback> ();
		//killerActions = killer.GetComponent<PlayerActions> ();
		killerData = killer.GetComponent<PlayerData> ();
		//killerMove = killer.GetComponent<PlayerMovement> ();
		killerAudio = killer.GetComponent<AudioSource> ();

		//victimFeedback = victim.GetComponent<KillFeedback> ();
		//victimActions = victim.GetComponent<PlayerActions> ();
		victimData = victim.GetComponent<PlayerData> ();
		victimMove = victim.GetComponent<PlayerMovement> ();
		victimAudio = victim.GetComponent<AudioSource> ();
		victimVib = victim.GetComponent<ControllerVibration> ();
	}

	void successfulKillMethod (GameObject killer, GameObject victim)
	{
		GetReferences (killer, victim);
		killerData.MakeSheep ();
		//killerActions.isWolf = false;

		StartCoroutine (SuccessKill (timeSpentInWolfShape));
	}

	IEnumerator SuccessKill (float delay)
	{
		SoundManager.Instance.PauseMusic ();
		victimVib.KillVibration (delay);
		killerFeedback.ShapeShiftFeedback (delay);
		killerFeedback.SlowMoFeedback (victimMove, delay);
		SoundManager.Instance.PlaySuccessKillSound (killerAudio, victimAudio, delay);

		yield return new WaitForSeconds (delay);

		SoundManager.Instance.UnPauseMusic ();
		spawnManager.RespawnPlayer (victimData);
		scoreManager.SuccessfulKillScoreUpdate (killerData, victimData);
		wolfManager.CreateRandomWolf ();
	}

	void unsuccessfulKillMethod (GameObject killer, NPSheep victim)
	{
		killerFeedback = killer.GetComponent<KillFeedback> ();
		//killerActions = killer.GetComponent<PlayerActions> ();
		killerData = killer.GetComponent<PlayerData> ();
		killerAudio = killer.GetComponent<AudioSource> ();

		//killerActions.isWolf = false;
		killerData.MakeSheep ();

		victim.TakeDamage (victim);
		victim.CamShake ();
		killerFeedback.ShapeShiftFeedback (timeSpentInWolfShape);
		SoundManager.Instance.PlayFailKillSound (killerAudio);
		wolfManager.CreateRandomWolf ();

		scoreManager.UnsuccessfulKillScoreUpdate (killerData);
	}

	void OnDisable()
	{
		PlayerActions.OnPlayerWasKilled -= successfulKillMethod;
		PlayerActions.OnNPSheepWasKilled -= unsuccessfulKillMethod;
	}
}
