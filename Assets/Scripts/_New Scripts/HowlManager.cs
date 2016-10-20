using UnityEngine;
using System.Collections.Generic;

public class HowlManager : MonoBehaviour
{
	List <GameObject> npSheepToCheckForScatter;

	void OnEnable()
	{
		PlayerController.OnWolfHowled += ScatterNPSheep;
		PlayerController.OnWolfHowled += CoolDownUI;
	}

	void ScatterNPSheep (PlayerController wolf)
	{
		npSheepToCheckForScatter = FindObjectOfType<NPSheepSpawner> ().npSheepInGame;

		for (int i = 0 ; i < npSheepToCheckForScatter.Count ; i++)
		{
			npSheepToCheckForScatter [i].GetComponent<NPSheep> ().CheckDistanceFromWolf (wolf);
		}

		SoundManager.Instance.PlayWolfHowl (wolf);
		SoundManager.Instance.PlaySheepReactionToHowl (wolf);
	}

	void CoolDownUI (PlayerController wolf)
	{
		GameObject.Find ("HowlUI").GetComponent<HowlCoolDownUI> ().enabled = true;
		GameObject.Find ("HowlUI").GetComponent<HowlCoolDownUI> ().coolDownTime = wolf.howlCooldownTime;
	}

	void OnDisable()
	{
		PlayerController.OnWolfHowled -= ScatterNPSheep;
		PlayerController.OnWolfHowled -= CoolDownUI;
	}
}
