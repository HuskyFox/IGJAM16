using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HowlManager : MonoBehaviour
{
	[HideInInspector] public bool currentlyPlaying;

	[SerializeField] private float _howlCooldownTime = 5f;
	[SerializeField] private float _howlReach = 3f;
	[SerializeField] private HowlCoolDownUI _UI;
	[SerializeField] private Button _restartButton;
	private PlayerActions _wolf = null;						//assigned when a player howls.
	private int _hittableMask = 9;							//"Hittable" is the 8th Layer.

	void OnEnable()
	{
		_hittableMask = ~_hittableMask;
		currentlyPlaying = true;

		PlayerActions.OnWolfHowled += ScatterNPSheep;
		PlayerActions.OnWolfHowled += CoolDownUI;
	}

	void ScatterNPSheep (PlayerActions wolf)
	{ 
		currentlyPlaying = true;
		_restartButton.interactable = false;	//to avoid an error in ParadoxNotion.

		_wolf = wolf;

		SoundManager.Instance.PlayWolfHowl (wolf);
		SoundManager.Instance.PlaySheepReactionToHowl (wolf);

		StartCoroutine (Scatter ());
	}

	IEnumerator Scatter()
	{
		//the scatter logic is called 8 times, 4 times per second, so that it lasts for 2 secs. We can tweak that if it's too long.
		int _counter = 8;
		while(_counter >= 0)
		{
			ScatterLogic ();

			if (!currentlyPlaying)
				yield return StartCoroutine (WaitForUnpause ());

			yield return new WaitForSeconds (0.25f);

			_counter -= 1;
		}

		//timer to let the sheep go back to "wander" state before re-enabling the restart function.
		float timer = 1f;
		while (timer > 0)
		{
			if (!currentlyPlaying)
				yield return StartCoroutine (WaitForUnpause ());

			yield return null;

			timer -= Time.deltaTime;
		}

		_restartButton.interactable = true;
		print ("Howl finished");
	}

	void ScatterLogic()
	{
		//check for the colliders inside the sphere of howlReach radius.
		Collider[] npSheepAround = Physics.OverlapSphere (_wolf.transform.position, _howlReach, _hittableMask);
		for(int i = 0 ; i < npSheepAround.Length ; i++)
		{
			if(npSheepAround[i].tag == "NPSheep")
			{
				npSheepAround [i].GetComponent<NPSheep> ().RunAway (_wolf.transform);	//order the NPSheep to run away.
			}
		}
	}

	void CoolDownUI (PlayerActions wolf)
	{
		StartCoroutine (WaitForCooldown (wolf));
	}

	IEnumerator WaitForCooldown(PlayerActions wolf)
	{
		//Wait until the bar is filled up.
		yield return StartCoroutine (_UI.FillUp (_howlCooldownTime));

		//Enable the howl again.
		wolf.canHowl = true;
	}

	IEnumerator WaitForUnpause()
	{
		while (!currentlyPlaying)
		{
			yield return null;
		}
	}

	void OnDisable()
	{
		PlayerActions.OnWolfHowled -= ScatterNPSheep;
		PlayerActions.OnWolfHowled -= CoolDownUI;
	}
}
