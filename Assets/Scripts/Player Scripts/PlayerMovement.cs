using UnityEngine;
using System.Collections;
using InControl;

public class PlayerMovement : MonoBehaviour 
{
	[SerializeField] private float _speed = 4.5f;
	[SerializeField] private float _rotateSpeed = 10f;
	[SerializeField] private Animator _sheepAnimator;
	[SerializeField] private Animator _wolfAnimator;
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
		//Prevent movement when the game is paused
		if (_gamePaused)
			return;
		
		//stores the inputs of the controller
		float h = _controller.LeftStickX;
		float v = _controller.LeftStickY;

		//defines a vector from those inputs
		_movement.Set (h, 0f, v);

		//Calls the movement and animation functions, passing in the vector.
		Move (_movement);
		Rotate (_movement);
		Animate (h, v);
	}

	void Move (Vector3 movement)
	{
		movement = movement * _speed * Time.deltaTime;
		_rb.MovePosition (transform.position + movement);
	}

	void Rotate(Vector3 direction)
	{
		if (direction != Vector3.zero)
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * _rotateSpeed);
	}

	void Animate (float h, float v)
	{
		_isMoving = Mathf.Abs(h) + Mathf.Abs(v) > 0.05f;
		if (!_sheepAnimator) return;
		_sheepAnimator.SetBool("Moving", _isMoving);
		if (!_wolfAnimator)
			return;
		_wolfAnimator.SetBool ("Moving", _isMoving);
	}

	public void PauseToggle ()
	{
		_gamePaused = !_gamePaused;

		//Enable/disable the animator to pause/unpause the animation.
		_sheepAnimator.enabled = !_sheepAnimator.isActiveAndEnabled;
		_wolfAnimator.enabled = !_wolfAnimator.isActiveAndEnabled;
	}

	void OnDisable()
	{
		_sheepAnimator.SetBool ("Moving", false);
	}
}
