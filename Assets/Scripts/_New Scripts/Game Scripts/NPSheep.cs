using UnityEngine;
using System.Collections;
using NodeCanvas.StateMachines;

public class NPSheep : MonoBehaviour 
{
	[SerializeField] GameObject killParticles;
	FSMOwner behavior;
	Animator anim;

	void Awake()
	{
		behavior = GetComponent<FSMOwner> ();
		anim = GetComponent<Animator> ();
	}

	void OnEnable()
	{
		behavior.enabled = true;
		anim.enabled = true;
	}

	public void TakeDamage (NPSheep victim)
	{
		var particles = Instantiate(killParticles);
		if (particles) particles.transform.position = transform.position;
		gameObject.SetActive (false);
		GetComponentInParent<NPSheepSpawner>().npSheepInGame.Remove (gameObject);
	}

	public void CheckDistanceFromWolf (PlayerActions wolf)
	{
		var distanceFromWolf = Vector3.Distance(transform.position, wolf.transform.position);
		if (distanceFromWolf < wolf.howlReach)
			behavior.blackboard.SetValue ("Threat", wolf.gameObject.transform);
	}

	public void PauseToggle()
	{
		behavior.enabled = !behavior.enabled;
		anim.enabled = !anim.enabled;
	}
}
