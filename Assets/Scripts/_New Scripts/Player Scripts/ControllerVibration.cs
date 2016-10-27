using UnityEngine;
using System.Collections;
using InControl;

public class ControllerVibration : MonoBehaviour 
{
	public bool isWolf;
	InputDevice controller;

	void Start()
	{
		controller = GetComponent<PlayerData> ().controller;
	}

	void Update()
	{
		if (isWolf)
			controller.Vibrate (0.1f, 0.1f);
		else
			controller.StopVibration ();
	}

	public void KillVibration(float duration)
	{
		StartCoroutine (Vibration (duration));
	}

	IEnumerator Vibration (float duration)
	{
		isWolf = true;
		float start = Time.realtimeSinceStartup;

		controller.Vibrate (0.2f, 0.2f);

		while (Time.realtimeSinceStartup < start + duration)
		{
			yield return null;
		}

		isWolf = false;
		//controller.StopVibration ();
	}

	void OnDisable()
	{
		controller.StopVibration ();
	}

}
