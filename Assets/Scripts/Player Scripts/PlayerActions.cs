using UnityEngine;
using System.Collections;
using InControl;

public class PlayerActions : MonoBehaviour
{
	[HideInInspector] public bool isWolf = false;
	[HideInInspector] public bool canHowl = false;
	[SerializeField]private PlayerData _data;
	private InputDevice _controller;
	private bool _gamePaused = false;

	//variables for ATTACK
	[SerializeField][Range (0, 3)] private float _attackRange = 0.75f;	//radius of the OverlapSphere in the attack function
	[SerializeField] private Transform _attackSphereOrigin;	//origin of the OverlapSphere
	private int _hittableMask = 9;	//"Hittable" is the 8th Layer.

	//variables for HOWL
	public float howlCooldownTime = 5f;
	public float howlReach = 3f;
	[SerializeField] private ParticleSystem _howlParticles;

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
		_hittableMask = ~_hittableMask;
		_controller = _data.controller;
	}

	void Update()
	{
		//prevent the actions when the game is paused.
		if (_gamePaused)
			return;
		
		Controls ();
	}

	void Controls()
	{	
		//WOLF CONTROLS
		if(isWolf)
		{
			if (_controller.Action1.WasPressed)
				Attack ();

			if (_controller.Action2.WasPressed && canHowl)
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
		//Get the position of the player when the action key was pressed.
		Vector3 attackPos = _attackSphereOrigin.transform.position;

		//draw a ray in the scene view, corresponding to the radius of the OverlapSphere
		//Debug.DrawRay (attackPos, transform.forward * _attackRange, Color.red, 3f);

		//get an array of the colliders that were inside the OverlapSphere.
		Collider[] hitColliders = Physics.OverlapSphere (attackPos, _attackRange, _hittableMask);

		//check the colliders that were hit when the action key was pressed...
		for (int i = 0; i < hitColliders.Length; i++) 
		{
			Collider hitCollider = hitColliders [i].GetComponent<Collider> ();

			//and checks their tag.
			if (hitCollider.tag == "PlayerSheep") 
			{
				//Activate the event OnPlayerWasKilled
				//and pass in this GameObject for the killer
				//and the GameObject correspondant to the collider that was hit for the victim.
				if (OnPlayerWasKilled != null)
					OnPlayerWasKilled (gameObject, hitCollider.gameObject);
				print (name + " killed " + hitCollider.name + "!");
				return;	//to only kill one player if there were several colliders.
			} 
			else if (hitCollider.tag == "NPSheep")
			{
				//Activate the event OnNPSheepWasKilled
				//and pass in this GameObject for the killer
				//and the NPSheep script correspondant to the collider that was hit for the victim.
				NPSheep victim = hitCollider.GetComponent<NPSheep> ();
				if (OnNPSheepWasKilled != null)
					OnNPSheepWasKilled (gameObject, victim);
				print (name + " killed a NPSheep!");
				return;	//to only kill one sheep if there were several colliders.
			} 
		}
	}

	void Howl()
	{
		_howlParticles.Play ();
		canHowl = false;	//Wait for the howl to cooldown (see HowlManager script).

		//GetComponent<KillFeedback> ().ShapeShiftFeedback (this.GetComponent<PlayerController>());

		if (OnWolfHowled != null)
			OnWolfHowled (this);
	}

	public void PauseToggle()
	{
		_gamePaused = !_gamePaused;
	}
}
