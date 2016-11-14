using UnityEngine;
using System.Collections;
using InControl;

public class PlayerMovement : MonoBehaviour 
{
	[SerializeField] float speed = 4.5f;
	[SerializeField] float rotateSpeed = 10f;
	[SerializeField] Animator sheepAnimator;
	[SerializeField] Animator wolfAnimator;

	Vector3 movement;
	Rigidbody rb;
	InputDevice controller;

	bool isMoving = false;

	void Start()
	{
		rb = GetComponent <Rigidbody> ();
		controller = GetComponent<PlayerData> ().controller;
	}

	void FixedUpdate()
	{	
		//stores the inputs of the device if there is one attached to the player
		float h = controller.LeftStickX;
		float v = controller.LeftStickY;
		//defines a vector from those inputs
		movement.Set (h, 0f, v);

		//if the player is not too high,
		//calls the functions controlling the movement of the player, passing in the inputs
		if (transform.position.y <= 2f) {
			Move (movement);
			Rotate (movement);
			Animate (h, v);
		}
	}

	void Move (Vector3 movement)
	{
		movement = movement * speed * Time.deltaTime;
		rb.MovePosition (transform.position + movement);
	}

	void Rotate(Vector3 direction)
	{
		if (direction != Vector3.zero)
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * rotateSpeed);
	}

	void Animate (float h, float v)
	{
		isMoving = Mathf.Abs(h) + Mathf.Abs(v) > 0.05f;
		if (!sheepAnimator) return;
		sheepAnimator.SetBool("Moving", isMoving);
		if (!wolfAnimator)
			return;
		wolfAnimator.SetBool ("Moving", isMoving);
	}

	void OnDisable()
	{
		sheepAnimator.SetBool ("Moving", false);
	}
}
