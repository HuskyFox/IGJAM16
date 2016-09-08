using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour 
{
	//Control variables editable in the Inspector
	public float speed = 4.5f;
	public float rotateSpeed = 10f;
	public float spawnHeight = 4f;
	public float attackRange = 0.5f;

	public Animator animationController;
	//public GameObject Particles;

	private Vector3 movement;
	private Rigidbody rigidbody;
	private int hittableMask = 1 << 8;

	[HideInInspector]public string playerIndex;	//does it need to be public ???

	public bool isWolf { get; set; }	//get and set the boolean when the function MakeWolf is called in the NewWolfManager.

	bool isGameStarted;	//used in Update

	public InputDevice Device { get; set;}	//get and set the device when the function "AssignDeviceToPlayer" is called in the DevicesManager

	//the two events that let other scripts know of the kill
	public delegate void PlayerWasKilled (PlayerController killer, PlayerController victim);
	public static event PlayerWasKilled OnPlayerWasKilled;

	public delegate void NPSheepWasKilled(PlayerController killer, Sheep victim);
	public static event NPSheepWasKilled OnNPSheepWasKilled;

	void Awake ()
	{
		rigidbody = GetComponent<Rigidbody> ();
		playerIndex = gameObject.name.Replace ("Player_", "");
	}
		
	void Update()
	{
		if(!isGameStarted && Device != null)
		{
			isGameStarted = true;
			RespawnPlayer ();
			//			groundIndicator.color = new Color(Random.value, Random.value, Random.value);
			//			Invoke("RemoveIndicator", 5);
		}

		//check if there's a device attached to the player
		if (Device == null)
			return;
		
		if (isGameStarted)
			Controls ();

		//makes the controller vibrate when the player is the wolf.
		if (isWolf)
			Device.Vibrate (0.1f, 0.1f);
		else
			Device.StopVibration ();
	}

	//function called to spawn the players
	public void RespawnPlayer()
	{
//		var particles = Instantiate (Particles);
//		if (particles) particles.transform.position = transform.position;
		
		//assigns the right spawn position depending on the player index
		Vector3 startPosition = GameObject.Find ("Plane"+playerIndex).transform.position;
		startPosition.y = spawnHeight;
		transform.position = startPosition;
	}

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
		Vector3 attackPos = transform.position;
		attackPos += movement * (attackRange / 1.5f);

		Collider[] hitColliders = Physics.OverlapSphere (attackPos, attackRange, hittableMask);

		//checks the colliders that were hit when the action key was pressed
		for (int i = 0 ; i < hitColliders.Length ; i++)
		{
			if (hitColliders [i].tag == "PlayerSheep") 
			{
				//enables the KillManager
				FindObjectOfType<KillManager> ().enabled = true;

				//Activate the event OnPlayerWasKilled
				//and passes in this script for the killer
				//and the script correspondant to the player whose collider was hit.
				PlayerController killer = this.GetComponent<PlayerController> ();
				PlayerController victim = hitColliders [i].GetComponent<PlayerController> ();
				OnPlayerWasKilled (killer, victim);
				print ("You killed Player!");
			} 
			else if (hitColliders [i].tag == "NPSheep") 
			{
				//enables the KillManager
				FindObjectOfType<KillManager> ().enabled = true;

				//Activates the event OnNPSheepWasKilled
				//and passes in this script for the killer
				//and the script correspondant to the NPSheep whose collider was hit.		
				PlayerController killer = this.GetComponent<PlayerController> ();
				Sheep victim = hitColliders [i].GetComponent<Sheep> ();
				OnNPSheepWasKilled (killer, victim);
				print ("You killed a NPSheep!");
			} 
			//else
			//	return;	//not sure this is needed...
			
			return;	//to only kill one sheep.
		}
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
		if (!animationController) return;
		animationController.SetBool("Moving", isMoving);
	}
}
