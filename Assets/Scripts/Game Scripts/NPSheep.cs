using UnityEngine;
using System.Collections;
using NodeCanvas.StateMachines;

/* This script is attached to the NPSheep prefab.
 * The wandering is handled by the FSM (can be modify with the Paradox Notion tool).*/
public class NPSheep : MonoBehaviour 
{
	[SerializeField] private GameObject _killParticles;
	[SerializeField] private GameObject _shape;
	private CapsuleCollider _coll;
	private FSMOwner _behavior;
	private Animator _anim;
	private AudioSource _audio;
	private float _distanceFromWolf;

	void Awake()
	{
		_behavior = GetComponent<FSMOwner> ();
		_anim = GetComponent<Animator> ();
		_audio = GetComponent<AudioSource> ();
		_coll = GetComponent<CapsuleCollider> ();
	}

	void OnEnable()
	{
		_behavior.enabled = true;
		_anim.enabled = true;
		_coll.enabled = true;
		_shape.SetActive (true);
	}

	// Called by the KillManager script.
	public IEnumerator TakeDamage()
	{
		SoundManager.Instance.PlayKillSound (_audio, "Poof");

		var particles = Instantiate(_killParticles);
		if (particles) particles.transform.position = transform.position;

		//Hide the NPSheep but leave him active, if not the sound and particles won't play.
		_coll.enabled = false;
		_shape.SetActive (false);

		yield return new WaitForSeconds (2f);

		// Currently set the NPSheep inactive and remove it from the list.
		// Change something here if we decide to respawn them instead of removing them.
		gameObject.SetActive (false);
		GetComponentInParent<NPSheepSpawner>().npSheepInGame.Remove (gameObject);
	}

	public void RunAway(Transform wolf)
	{
		_behavior.blackboard.SetValue ("Threat", wolf);
	}

	// Called by the GameStateManager script when the game is paused.
	public void PauseToggle()
	{
		_behavior.enabled = !_behavior.enabled;
		_anim.enabled = !_anim.enabled;
	}
}
