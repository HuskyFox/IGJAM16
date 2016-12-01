using UnityEngine;
using System.Collections;
using InControl;

public class PlayerMovement : MonoBehaviour 
{
	[SerializeField] float speed = 4.5f;
	[SerializeField] float rotateSpeed = 10f;
	[SerializeField] Animator sheepAnimator;
	[SerializeField] Animator wolfAnimator;

	private Vector3 _movement;
	private Rigidbody _rb;
	private InputDevice _controller;

	private bool _isMoving = false;
	private bool _gamePaused = false;

	void Start()
	{
		_rb = GetComponent <Rigidbody> ();
		_controller = GetComponent<PlayerData> ().controller;
	}

	void FixedUpdate()
	{	
		if (_gamePaused)
			return;
		
		//stores the inputs of the device if there is one attached to the player
		float h = _controller.LeftStickX;
		float v = _controller.LeftStickY;
		//defines a vector from those inputs
		_movement.Set (h, 0f, v);

		Move (_movement);
		Rotate (_movement);
		Animate (h, v);
	}

	void Move (Vector3 movement)
	{
		movement = movement * speed * Time.deltaTime;
		_rb.MovePosition (transform.position + movement);
	}

	void Rotate(Vector3 direction)
	{
		if (direction != Vector3.zero)
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * rotateSpeed);
	}

	void Animate (float h, float v)
	{
		_isMoving = Mathf.Abs(h) + Mathf.Abs(v) > 0.05f;
		if (!sheepAnimator) return;
		sheepAnimator.SetBool("Moving", _isMoving);
		if (!wolfAnimator)
			return;
		wolfAnimator.SetBool ("Moving", _isMoving);
	}

	public void PauseToggle ()
	{
		_gamePaused = !_gamePaused;

		sheepAnimator.enabled = !sheepAnimator.isActiveAndEnabled;
		wolfAnimator.enabled = !wolfAnimator.isActiveAndEnabled;
	}

	void OnDisable()
	{
		sheepAnimator.SetBool ("Moving", false);
	}
}
