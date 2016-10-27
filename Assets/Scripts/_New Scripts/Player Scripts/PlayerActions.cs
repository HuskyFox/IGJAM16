using UnityEngine;
using System.Collections;
using InControl;

public class PlayerActions : MonoBehaviour
{
	public bool isWolf = false;
	InputDevice controller;
	[SerializeField]PlayerData data;

	//variables for ATTACK
	[SerializeField]
	[Range (0, 3)]
	float attackRange = 0.75f;	//radius of the OverlapSphere in the attack function / lenght of the raycast
	[SerializeField]
	Transform attackSphereOrigin;	//origin of the OverlapSphere

	int hittableMask = 9;	//"Hittable" is the 8th Layer.

	//variables for HOWL
	public float howlCooldownTime = 5f;
	public float howlReach = 5f;
	//[SerializeField]
	//float howlDuration = 2f;
	[SerializeField]
	ParticleSystem howlParticles;
	private float _elapsedTime;

	//the two events that let other scripts know of the kill
	public delegate void PlayerWasKilled (GameObject killer, GameObject victim);
	public static event PlayerWasKilled OnPlayerWasKilled;

	public delegate void NPSheepWasKilled(GameObject killer, NPSheep victim);
	public static event NPSheepWasKilled OnNPSheepWasKilled;

	//the event that broadcast the howl threat to the NPSheep
	public delegate void WolfHowled (PlayerActions wolf);
	public static event WolfHowled OnWolfHowled;


	void Start()
	{
		hittableMask = ~hittableMask;
		_elapsedTime = howlCooldownTime;
		controller = data.controller;
	}

	void Update()
	{
		Controls ();

		if(isWolf)
			_elapsedTime += Time.deltaTime;
	}

	void Controls()
	{	
		//WOLF CONTROLS
		if(isWolf)
		{
			if (controller.Action1.WasPressed)
				Attack ();

			if(controller.Action2.WasPressed && _elapsedTime >= howlCooldownTime) 
				Howl ();

			//other actions
		}

		//SHEEP CONTROLS (if we want to allow actions as a sheep)
		/*if(!isWolf)
		{
			
		}*/
	}

	void Attack()
	{
		Vector3 attackPos = attackSphereOrigin.transform.position;

		//draws a ray corresponding to the radius of the OverlapSphere
		Debug.DrawRay (attackPos, transform.forward * attackRange, Color.red, 3f);

		//get an array of the colliders that were inside the OverlapSphere
		Collider[] hitColliders = Physics.OverlapSphere (attackPos, attackRange, hittableMask);

		//checks the colliders that were hit when the action key was pressed...
		for (int i = 0; i < hitColliders.Length; i++) {
			Collider hitCollider = hitColliders [i].GetComponent<Collider> ();

			//and checks its tag.
			if (hitCollider.tag == "PlayerSheep") {
				//Activate the event OnPlayerWasKilled
				//and passes in this GameObject for the killer
				//and the GameObject correspondant to the collider that was hit for the victim.
				if (OnPlayerWasKilled != null)
					OnPlayerWasKilled (gameObject, hitCollider.gameObject);
				//isWolf = false;
				print (name + " killed " + hitCollider.name + "!");
				return;	//to only kill one player if there were several colliders.
			} else if (hitCollider.tag == "NPSheep") {
				//Activate the event OnNPSheepWasKilled
				//and passes in this GameObject for the killer
				//and the NPSheep script correspondant to the collider that was hit for the victim.
				NPSheep victim = hitCollider.GetComponent<NPSheep> ();
				if (OnNPSheepWasKilled != null)
					OnNPSheepWasKilled (gameObject, victim);
				//isWolf = false;
				print (name + " killed a NPSheep!");
				return;	//to only kill one sheep if there were several colliders.
			} 
		}
	}

	void Howl()
	{
		howlParticles.Play ();
		_elapsedTime = 0f;

		//GetComponent<KillFeedback> ().ShapeShiftFeedback (this.GetComponent<PlayerController>());

		if (OnWolfHowled != null)
			OnWolfHowled (this);
	}
}
