using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	//Control variables editable in the Inspector
	public float speed = 4.5f;
	public float rotateSpeed = 10f;
	//public float spawnHeight = 4f;
	[Range (0, 3)]
	public float attackRange = 0.75f;	//radius of the OverlapSphere in the attack function / lenght of the raycast

	public Animator sheepAnimator;
	private Animator wolfAnimator;
	//public GameObject Particles;

	//variables for the movement
	private Vector3 movement;
	private Rigidbody rigidbody;

	//variables for the attack function
	private int hittableMask = 9;	//"Hittable" is the 8th Layer.
	Transform attackSphereOrigin;	//origin of the OverlapSphere

	//public string playerIndex;	//does it need to be public ???

	public bool isWolf { get; set; }	//get and set the boolean when the function MakeWolf is called in the NewWolfManager.

	bool isGameStarted;	//used in Update

	public InputDevice Device { get; set;}	//get and set the device when the function "AssignDeviceToPlayer" is called in the DevicesManager

	[HideInInspector]public GameObject score;
	public int scoreKeeper { get; set;}

	//the two events that let other scripts know of the kill
	public delegate void PlayerWasKilled (PlayerController killer, PlayerController victim);
	public static event PlayerWasKilled OnPlayerWasKilled;

	public delegate void NPSheepWasKilled(PlayerController killer, NPSheep victim);
	public static event NPSheepWasKilled OnNPSheepWasKilled;

	void Awake ()
	{
		rigidbody = GetComponent <Rigidbody> ();
		//playerIndex = gameObject.name.Replace ("Player_", "");
		hittableMask = ~hittableMask;
		attackSphereOrigin = transform.Find ("Sheep/AttackSphereOrigin");
		wolfAnimator = transform.Find ("Wolf").GetComponent<Animator> ();
		score = GameObject.Find("Player Scores").transform.Find("Score" + gameObject.name).gameObject;
		//score = GameObject.Find ("Score" + gameObject.name).GetComponent<Text>();
	}
		
	void Update()
	{
//		if(!isGameStarted && Device != null)
//		{
//			isGameStarted = true;
//			//RespawnPlayer ();
//			//			groundIndicator.color = new Color(Random.value, Random.value, Random.value);
//			//			Invoke("RemoveIndicator", 5);
//		}

		//check if there's a device attached to the player
		if (Device == null || !GameStateManager.gameOn)
			return;
		
		Controls ();

		//makes the controller vibrate when the player is the wolf.
		if (isWolf)
			Device.Vibrate (0.1f, 0.1f);
		else
			Device.StopVibration ();
	}

	//function called to spawn the players
//	public void RespawnPlayer()
//	{
////		var particles = Instantiate (Particles);
////		if (particles) particles.transform.position = transform.position;
//		
//		//assigns the right spawn position depending on the player index
//		Vector3 spawnPosition = GameObject.Find ("SpawnPositionPlayer_"+playerIndex).transform.position;
//		spawnPosition.y = spawnHeight;
//		transform.position = spawnPosition;
//	}

	public void Controls()
	{	
		//WOLF CONTROLS
		if(isWolf)
		{
			if (Device.Action1.WasPressed)
				Attack ();

			if(Device.Action2.WasPressed){}
				//Howl();
			//other actions
		}

		//SHEEP CONTROLS (if we want to allow actions as a sheep)
		/*if(isSheep)
		{
			
		}*/
	}

	public void Attack()
	{
		//original attack system with OverlapSphere

		Vector3 attackPos = attackSphereOrigin.transform.position;

		//draws a ray corresponding to the radius of the OverlapSphere
		Debug.DrawRay (attackPos, transform.forward * attackRange, Color.red, 3f);

		//get an array of the colliders that were inside the OverlapSphere
		Collider[] hitColliders = Physics.OverlapSphere (attackPos, attackRange, hittableMask);

		//checks the colliders that were hit when the action key was pressed...
		for (int i = 0 ; i < hitColliders.Length ; i++)
		{
			Collider hitCollider = hitColliders [i].GetComponent<Collider> ();

			//and checks its tag.
			if (hitCollider.tag == "PlayerSheep") 
			{
				//enables the KillManager
				FindObjectOfType<KillManager> ().enabled = true;

				//Activate the event OnPlayerWasKilled
				//and passes in this script for the killer
				//and the script correspondant to the player whose collider was hit.
				PlayerController killer = this.GetComponent<PlayerController> ();
				PlayerController victim = hitCollider.GetComponent<PlayerController> ();
				if(OnPlayerWasKilled != null)
					OnPlayerWasKilled (killer, victim);
				print ("You killed a Player!");
				return;	//to only kill one player if there were several colliders.
			} 
			else if (hitCollider.tag == "NPSheep") 
			{
				//enables the KillManager
				FindObjectOfType<KillManager> ().enabled = true;

				//Activates the event OnNPSheepWasKilled
				//and passes in this script for the killer
				//and the script correspondant to the NPSheep whose collider was hit.		
				PlayerController killer = this.GetComponent<PlayerController> ();
				NPSheep victim = hitCollider.GetComponent<NPSheep> ();
				if(OnNPSheepWasKilled != null)
					OnNPSheepWasKilled (killer, victim);
				print ("You killed a NPSheep!");
				return;	//to only kill one sheep if there were several colliders.
			} 
		}

		//new RayCast attack system
		/*
		//declaration of the origin of the Ray
		Vector3 hitOrigin = transform.position;
		//moves the origin up on the y axis (the player is at -0.02 for some reason)
		hitOrigin.y += 0.1f;	//we could do hitOrigin.y = 0f; but then we don't see the ray in DrawRay.

		Ray hitRay = new Ray (hitOrigin, transform.forward);	//from where the player is, in the direction he's facing.
		RaycastHit hit;
		Debug.DrawRay (hitOrigin, transform.forward * attackRange, Color.red, 2f);

		//if the RayCast hits something...
		if (Physics.Raycast (hitRay, out hit, attackRange, hittableMask))
		{
			//we get a reference to the collider that was hit...
			Collider hitCollider = hit.collider.GetComponent<Collider> ();

			//and we check its tag.
			if (hitCollider.tag == "PlayerSheep") 
			{
				//enables the KillManager
				FindObjectOfType<KillManager> ().enabled = true;

				//Activate the event OnPlayerWasKilled
				//and passes in this script for the killer
				//and the script correspondant to the player whose collider was hit.
				PlayerController killer = this.GetComponent<PlayerController> ();	//or just this; ?
				PlayerController victim = hitCollider.GetComponent<PlayerController> ();
				OnPlayerWasKilled (killer, victim);
				print ("You killed a Player!");
				return;
			} 
			else if (hitCollider.tag == "NPSheep") 
			{
				//enables the KillManager
				FindObjectOfType<KillManager> ().enabled = true;

				//Activates the event OnNPSheepWasKilled
				//and passes in this script for the killer
				//and the script correspondant to the NPSheep whose collider was hit.		
				PlayerController killer = this.GetComponent<PlayerController> ();	//or just this; ?
				NPSheep victim = hitCollider.GetComponent<NPSheep> ();
				OnNPSheepWasKilled (killer, victim);
				print ("You killed a NPSheep!");
				return;
			}
		}*/
	}
		
	void FixedUpdate()
	{	
		if (Device != null)
		{
			//stores the inputs of the device if there is one attached to the player
			float h = Device.LeftStickX;
			float v = Device.LeftStickY;
			//defines a vector from those inputs
			movement.Set (h, 0f, v);

			//if the player is not too high,
			//calls the functions controlling the movement of the player, passing in the inputs
			if (transform.position.y <= 2f) 
			{
				Move (movement);
				Rotate (movement);
				Animate (h, v);
			}
		}
	}

	void Move (Vector3 movement)
	{
		movement = movement * speed * Time.deltaTime;
		rigidbody.MovePosition (transform.position + movement);
	}

	void Rotate(Vector3 direction)
	{
		if (direction != Vector3.zero)
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * rotateSpeed);
	}

	void Animate (float h, float v)
	{
		var isMoving = false;
		isMoving = Mathf.Abs(h) + Mathf.Abs(v) > 0.05f;
		if (!sheepAnimator) return;
		sheepAnimator.SetBool("Moving", isMoving);
		if (!wolfAnimator)
			return;
		wolfAnimator.SetBool ("Moving", isMoving);
	}
}
