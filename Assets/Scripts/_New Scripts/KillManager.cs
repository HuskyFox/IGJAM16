using UnityEngine;
using System.Collections;

public class KillManager : MonoBehaviour
{
	void OnEnable()
	{
		PlayerController.OnPlayerWasKilled += successfulKillMethod;
		PlayerController.OnNPSheepWasKilled += unsuccessfulKillMethod;
	}

	void successfulKillMethod (PlayerController killer, PlayerController victim)
	{
		killer.GetComponent<KillFeedback> ().ShapeShiftFeedback (killer);
		killer.GetComponent<KillFeedback> ().SlowMoFeedback (killer, victim);
		TimeManager.Instance.NewWolfCountdownUI ();
		NewWolfManager.Instance.CreateRandomWolf ();

		//we might want to have this after the feedback animation, not right at the moment of the kill...
		FindObjectOfType<ScoreManager> ().SuccessfulKillScoreUpdate (killer, victim);

		this.enabled = false;
	}

	void unsuccessfulKillMethod (PlayerController killer, NPSheep victim)
	{
		victim.TakeDamage (victim);
		victim.CamShake ();
		killer.GetComponent<KillFeedback> ().ShapeShiftFeedback (killer);
		TimeManager.Instance.NewWolfCountdownUI ();
		NewWolfManager.Instance.CreateRandomWolf ();

		//we might want to have this after the feedback animation, not right at the moment of the kill...
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
