using UnityEngine;
using System.Collections;

public class TitleScreenObject : MonoBehaviour {


	public bool mustMove = false;
	public bool mustRotate = false;
	public bool mustApplyForce = false;

	public Vector3 moveSpeed;
	public float rotateSpeed;

	private float isMoving;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (mustMove) {
			Vector3 currentPosition = transform.position;
			currentPosition.x += moveSpeed.x * Time.deltaTime;
			currentPosition.y += moveSpeed.y * Time.deltaTime;
			currentPosition.z += moveSpeed.z * Time.deltaTime;
			transform.position = currentPosition;
		}

		if (mustRotate) {
			Vector3 currentRotation = transform.localEulerAngles;
			currentRotation.z += rotateSpeed * Time.deltaTime;
			transform.localEulerAngles = currentRotation;
		}

		if (mustApplyForce) {
			Rigidbody2D rb = GetComponent<Rigidbody2D> ();
			rb.AddForce ( Vector3.left*1000 );
		}
	}
}
