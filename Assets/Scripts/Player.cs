using InControl;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 6000f;

	public InputDevice Device { get; set; }

	Renderer cachedRenderer;

	private Rigidbody rigidbody;

	public bool isWolf { get; set; }

	private Vector3 hitDirection;
	private float attackRange = 0.5f;

	private int hittableMask = 1 << 8;

	// Use this for initialization
	void Start () {
		MakeWolf ();
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update() {
		if (isWolf) {
			if (Device.AnyButton.IsPressed) {
				Attack ();
			}
		}
	}

	public void MakeWolf() {
		isWolf = true;
		tag = "Wolf";
	}

	void Attack() {
		Vector3 attackPos = transform.position;
		hitDirection = Vector3.zero;
		attackPos += hitDirection * (attackRange/1.5f);
		Collider[] hitColliders = Physics.OverlapSphere(attackPos, attackRange, hittableMask);
		for(int i = 0; i < hitColliders.Length; i++){
			if (hitColliders [i].tag != "Wolf") {
				hitColliders[i].SendMessage("TakeDamage");
			}

		}
	}

	void TakeDamage() {
		print ("You killed a sheep!");
		//Destroy ( this.gameObject );
	}

	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.velocity = new Vector3(Device.LeftStickX * speed , 0.0f, Device.LeftStickY * speed);
	}
}
