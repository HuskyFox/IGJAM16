using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HowlManager : MonoBehaviour
{
	[SerializeField] private NPSheepSpawner _npSheep;
	[SerializeField] private HowlCoolDownUI _UI;
	private List <GameObject> _npSheepToCheckForScatter;

	void OnEnable()
	{
		PlayerActions.OnWolfHowled += ScatterNPSheep;
		PlayerActions.OnWolfHowled += CoolDownUI;
	}
		
	//This function could be directly in the NPSheep script, if we subscribe it to the howl event...
	void ScatterNPSheep (PlayerActions wolf)
	{
		//Get the current list of NPSheep
		_npSheepToCheckForScatter = _npSheep.npSheepInGame;

		for (int i = 0 ; i < _npSheepToCheckForScatter.Count ; i++)
		{
			_npSheepToCheckForScatter [i].GetComponent<NPSheep> ().CheckDistanceFromWolf (wolf);
		}

		SoundManager.Instance.PlayWolfHowl (wolf);
		SoundManager.Instance.PlaySheepReactionToHowl (wolf);
	}

	void CoolDownUI (PlayerActions wolf)
	{
		StartCoroutine (WaitForCooldown (wolf));
	}

	IEnumerator WaitForCooldown(PlayerActions wolf)
	{
		float cooldown = wolf.howlCooldownTime;		//get the cooldown time.

		//Wait until the bar is filled up.
		yield return StartCoroutine (_UI.FillUp (cooldown));

		//Enable the howl again.
		wolf.canHowl = true;
	}

	void OnDisable()
	{
		PlayerActions.OnWolfHowled -= ScatterNPSheep;
		PlayerActions.OnWolfHowled -= CoolDownUI;
	}
}
