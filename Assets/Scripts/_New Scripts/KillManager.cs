using UnityEngine;
using System.Collections;

public class KillManager : MonoBehaviour
{
	void OnEnable()
	{
		print ("KillManager enabled.");
		PlayerController.OnPlayerWasKilled += successfulKillMethod;
		PlayerController.OnNPSheepWasKilled += unsuccessfulKillMethod;
	}

	void successfulKillMethod (PlayerController killer, PlayerController victim)
	{
		killer.GetComponent<KillFeedback> ().ShapeShiftFeedback (killer);
		killer.GetComponent<KillFeedback> ().SlowMoFeedback (killer, victim);
		TimeManager.Instance.NewWolfCountdownUI ();
		NewWolfManager.Instance.CreateRandomWolf ();
		print ("Good kill!");
		this.enabled = false;
	}

	void unsuccessfulKillMethod (PlayerController killer, NPSheep victim)
	{
		victim.TakeDamage (victim);
		victim.CamShake ();
		killer.GetComponent<KillFeedback> ().ShapeShiftFeedback (killer);
		TimeManager.Instance.NewWolfCountdownUI ();
		NewWolfManager.Instance.CreateRandomWolf ();
		print ("Wrong kill!");
		this.enabled = false;
	}

	void OnDisable()
	{
		print ("KillManager disabled.");
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
