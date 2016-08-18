using InControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 6000f;
    public float rotateSpeed = 10f;

	public InputDevice Device { get; set; }

	Renderer cachedRenderer;

	private Rigidbody rigidbody;

	public bool isWolf { get; set; }

	private Vector3 faceDirection;
	private Vector3 hitDirection;
	private float attackRange = 0.5f;

	private int hittableMask = 1 << 8;

	private bool isKilled = false;
	private PlayerManager playerManager;

	private int score = 0;
	public string playerIndex;

	private bool isGameStarted = false;

	// Use this for initialization
	void Start () {
		Device = null;
		rigidbody = GetComponent<Rigidbody> ();
		playerManager = GameObject.Find ("PlayerManager").GetComponent<PlayerManager> ();
		playerIndex = gameObject.name.Replace ("Player_", "");
	}

	void Update() {
		if(!isGameStarted && SceneManager.GetActiveScene().name=="Demo Scene") {
			isGameStarted = true;
			Vector3 startPosition = GameObject.Find ("Plane"+playerIndex).transform.position;
			startPosition.y = 0;
			transform.position = startPosition;
		}

		if(isGameStarted)
			Controls();
	}

	void Controls() {
		if (isWolf) {
			if (Device.Action1.WasPressed) {
				Attack ();
			}
		}
	}

	public void MakeWolf() {
		isWolf = true;
		tag = "Wolf";
	}

	public void MakeSheep() {
		isWolf = false;
		tag = "Player";
	}

	void Attack() {
		Vector3 attackPos = transform.position;
		hitDirection = faceDirection;
	//	print (faceDirection);
		attackPos += hitDirection * (attackRange/1.5f);
		Collider[] hitColliders = Physics.OverlapSphere(attackPos, attackRange, hittableMask);
		for(int i = 0; i < hitColliders.Length; i++){
			if (hitColliders [i].tag != "Wolf") {

				hitColliders[i].SendMessage("TakeDamage",  this);
                return; //To only kill one
				//UpdatePlayerScoreGUI ();
			}
		}
	}

	void TakeDamage(object damageInflicter) {
		isKilled = true;
		playerManager.RespawnPlayer (gameObject.name);
		print ("You killed a sheep!");
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Device!=null) {
			faceDirection = new Vector3(Device.LeftStickX, 0.0f, Device.LeftStickY);
			RotateTowardsDirection(faceDirection);
			rigidbody.velocity = new Vector3(faceDirection.x * speed , 0.0f, faceDirection.z * speed);
		}

	}

    public void RotateTowardsDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                Time.deltaTime * rotateSpeed);
        }
    }
}
