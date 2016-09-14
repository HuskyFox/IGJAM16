using UnityEngine;
using System.Collections.Generic;

public class HowlManager : MonoBehaviour
{
	[HideInInspector]public List <GameObject> npSheepToCheckForScatter = new List <GameObject>();
	//public GameObject howlUI;

	void OnEnable()
	{
		PlayerController.OnWolfHowled += ScatterNPSheep;
		PlayerController.OnWolfHowled += CoolDownUI;
	}

	void ScatterNPSheep (PlayerController wolf)
	{
		for (int i = 0 ; i < npSheepToCheckForScatter.Count ; i++)
		{
			npSheepToCheckForScatter [i].GetComponent<NPSheep> ().CheckDistanceFromWolf (wolf);
		}
	}

	void CoolDownUI (PlayerController wolf)
	{
//		howlUI.GetComponent<HowlCoolDownUI> ().enabled = true;
		GameObject.Find ("HowlUI").GetComponent<HowlCoolDownUI> ().enabled = true;
		GameObject.Find ("HowlUI").GetComponent<HowlCoolDownUI> ().coolDownTime = wolf.howlCooldownTime;
	}

	void OnDisable()
	{
		PlayerController.OnWolfHowled -= ScatterNPSheep;
		PlayerController.OnWolfHowled -= CoolDownUI;
	}
}
