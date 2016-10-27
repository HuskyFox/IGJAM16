using UnityEngine;
using System.Collections.Generic;

public class HowlManager : MonoBehaviour
{
	[SerializeField]
	NPSheepSpawner npSheep;
	[SerializeField]
	HowlCoolDownUI UI;
	List <GameObject> npSheepToCheckForScatter;

	void OnEnable()
	{
		PlayerActions.OnWolfHowled += ScatterNPSheep;
		PlayerActions.OnWolfHowled += CoolDownUI;
	}

	void ScatterNPSheep (PlayerActions wolf)
	{
		npSheepToCheckForScatter = npSheep.npSheepInGame;

		for (int i = 0 ; i < npSheepToCheckForScatter.Count ; i++)
		{
			npSheepToCheckForScatter [i].GetComponent<NPSheep> ().CheckDistanceFromWolf (wolf);
		}

		SoundManager.Instance.PlayWolfHowl (wolf);
		SoundManager.Instance.PlaySheepReactionToHowl (wolf);
	}

	void CoolDownUI (PlayerActions wolf)
	{
		UI.enabled = true;
		UI.coolDownTime = wolf.howlCooldownTime;
	}

	void OnDisable()
	{
		PlayerActions.OnWolfHowled -= ScatterNPSheep;
		PlayerActions.OnWolfHowled -= CoolDownUI;
	}
}
