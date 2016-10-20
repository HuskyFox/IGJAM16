using UnityEngine;
using System.Collections;

public class KillManager : MonoBehaviour
{
	public float timeSpentInWolfShape = 0.3f;

	void OnEnable()
	{
		PlayerController.OnPlayerWasKilled += successfulKillMethod;
		PlayerController.OnNPSheepWasKilled += unsuccessfulKillMethod;
	}

	void successfulKillMethod (PlayerController killer, PlayerController victim)
	{
		killer.isWolf = false;

		StartCoroutine (SuccessKill (killer, victim, timeSpentInWolfShape));

		this.enabled = false;
	}

	IEnumerator SuccessKill (PlayerController killer, PlayerController victim, float delay)
	{
		SoundManager.Instance.PauseMusic ();
		killer.GetComponent<KillFeedback> ().VictimVibration (victim, delay);
		killer.GetComponent<KillFeedback> ().ShapeShiftFeedback (killer, delay);
		killer.GetComponent<KillFeedback> ().SlowMoFeedback (killer, victim, delay);
		SoundManager.Instance.PlaySuccessKillSound (killer, victim, delay);

		yield return new WaitForSeconds (delay);

		SoundManager.Instance.UnPauseMusic ();
		FindObjectOfType<ScoreManager> ().SuccessfulKillScoreUpdate (killer, victim);
		GetComponent<NewWolfManager>().CreateRandomWolf ();
	}

	void unsuccessfulKillMethod (PlayerController killer, NPSheep victim)
	{
		killer.isWolf = false;

		victim.TakeDamage (victim);
		victim.CamShake ();
		killer.GetComponent<KillFeedback> ().ShapeShiftFeedback (killer, timeSpentInWolfShape);
		SoundManager.Instance.PlayFailKillSound (killer);
		GetComponent<NewWolfManager>().CreateRandomWolf ();

		FindObjectOfType<ScoreManager> ().UnsuccessfulKillScoreUpdate (killer);
		this.enabled = false;
	}

	void OnDisable()
	{
		PlayerController.OnPlayerWasKilled -= successfulKillMethod;
		PlayerController.OnNPSheepWasKilled -= unsuccessfulKillMethod;
	}

//	public bool? successfulKill { get; set;}
//
//	void OnEnable()
//	{
//		print ("KillManager enabled.");
//		if ((bool)successfulKill)
//		{
//			print("Successful Kill!");
//			//OnPlayerWasKilled ();
//			successfulKill = null;
//		}
//		else if ((bool)!successfulKill)
//		{
//			print ("Wrong Kill!");
//			//OnNPSheepWasKilled ();
//			successfulKill = null;
//		}
//		this.enabled = false;
//	}
//
//	void OnDisable()
//	{
//		print("KillManager disabled.");
//	}
}
