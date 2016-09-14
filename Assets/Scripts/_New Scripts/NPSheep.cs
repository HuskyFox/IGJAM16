using UnityEngine;
using System.Collections;
using NodeCanvas.StateMachines;

public class NPSheep : MonoBehaviour 
{
//	void OnEnable()
//	{
//		PlayerController.OnWolfHowled += ScatterFromHowl;
//	}
//
//	void OnDisable()
//	{
//		PlayerController.OnWolfHowled -= ScatterFromHowl;
//	}

	public GameObject killParticles;

	public void TakeDamage (NPSheep victim)
	{
		var particles = Instantiate(killParticles);
		if (particles) particles.transform.position = transform.position;
		gameObject.SetActive (false);
	}

	public void CamShake()
	{
		var camShake = Camera.main.GetComponent<iTweenEvent>();
		if (camShake) Camera.main.GetComponent<iTweenEvent>().Play();
	}

	public void CheckDistanceFromWolf (PlayerController wolf)
	{
		var distanceFromWolf = Vector3.Distance(transform.position, wolf.transform.position);
		if (distanceFromWolf < wolf.howlReach)
			//StartCoroutine (Scatter (wolf, wolf.howlDuration));
			GetComponent<FSMOwner> ().blackboard.SetValue ("Threat", wolf.gameObject.transform);
	}

//	IEnumerator Scatter (PlayerController wolf, float duration)
//	{
//		float start = 0f;
//
//		while (start < duration)
//		{
//			GetComponent<FSMOwner> ().blackboard.SetValue ("Threat", wolf.gameObject.transform);
//			start += Time.deltaTime;
//			yield return null;
//		}
//	}
}
