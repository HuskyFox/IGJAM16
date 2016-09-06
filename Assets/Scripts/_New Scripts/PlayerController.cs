using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour 
{
	//Control variables editable in the Inspector
	public float speed = 4.5f;
	public float rotateSpeed = 10f;
	public float spawnHeight = 4f;

	public Animator animationController;

	private Vector3 movement;

	private Rigidbody rigidbody;

	[HideInInspector]public string playerIndex;	//does it need to be public ???

	bool isGameStarted;	//used in Update

	public InputDevice Device { get; set;}	//get and set the device when the function "AssignDeviceToPlayer" is called in the DevicesManager

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
	}

	//function called to spawn the players
	public void RespawnPlayer()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
//		var particles = Instantiate(Particles);
//		if (particles) particles.transform.position = transform.position;

		//assigns the right spawn position depending on the player index
		Vector3 startPosition = GameObject.Find ("Plane"+playerIndex).transform.position;
		startPosition.y = spawnHeight;
		transform.position = startPosition;
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

			//calls the functions controlling the movement of the player, passing in the inputs
			Move (movement);
			Rotate (movement);
			Animate (h, v);
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
