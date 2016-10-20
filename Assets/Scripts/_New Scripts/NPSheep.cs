using UnityEngine;
using System.Collections;
using NodeCanvas.StateMachines;

public class NPSheep : MonoBehaviour 
{
	public GameObject killParticles;

	public void TakeDamage (NPSheep victim)
	{
		var particles = Instantiate(killParticles);
		if (particles) particles.transform.position = transform.position;
		gameObject.SetActive (false);
		FindObjectOfType<NPSheepSpawner> ().npSheepInGame.Remove (gameObject);
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
			GetComponent<FSMOwner> ().blackboard.SetValue ("Threat", wolf.gameObject.transform);
	}
}
