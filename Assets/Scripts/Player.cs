using InControl;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 6000f;

	public InputDevice Device { get; set; }

	Renderer cachedRenderer;

	private Rigidbody rigidbody;

	public bool isWolf { get; set; }

	// Use this for initialization
	void Start () {
		isWolf = false;
		rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.velocity = new Vector3(Device.LeftStickX * speed , 0.0f, Device.LeftStickY * speed);
	}
}
