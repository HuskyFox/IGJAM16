using UnityEngine;
using System.Collections;

/* This script handles the audiovisual feedback related to the players when there is a kill.
 * The functions are called by the KillManager script.
 * The KillerFeedback method is overloaded, there is one for killing a player and one for killing a NPSheep.*/
public class KillFeedback : MonoBehaviour
{	
	[SerializeField] private float _wolfShapeDuration = 0.3f;
	[SerializeField] private bool _delay;
	[SerializeField] private float _delayDuration = 0.8f;
	[SerializeField] private Animator _wolfAnimator;
	[SerializeField] private GameObject _wolfShape;
	[SerializeField] private GameObject _sheepShape;
	[SerializeField] private GameObject _wolfArm;
	[SerializeField] private GameObject _hitParticles;
	[SerializeField] private GameObject _hitPow;
	private PlayerMovement _movement;
	private ControllerVibration _vibration;
	private AudioSource _audio;

	void Start()
	{
		_movement = GetComponent<PlayerMovement> ();
		_vibration = GetComponent<ControllerVibration> ();
		_audio = GetComponent<AudioSource> ();
	}

	//If the victim is a player, do the shape shift and the slowmo feedback.
	public IEnumerator KillerFeedback (KillFeedback victim)
	{
		//killer.transform.FindChild("NameTag").gameObject.SetActive(true);
		//victim.transform.FindChild("NameTag").gameObject.SetActive(true);

		//Switch to wolf shape.
		_sheepShape.SetActive (false);
		_wolfShape.SetActive (true);

		//Make the wolf face the victim.
		transform.LookAt (victim.transform.position);

		//Stop the player.
		_movement.enabled = false;

		//Trigger the attack animation.
		_wolfAnimator.SetTrigger("Attack");

		//Play growl sound.
		SoundManager.Instance.PlayKillSound (_audio, "Growl");

		// *** SLOW MOTION ***
		Time.timeScale = .1f;
		yield return new WaitForSeconds(_wolfShapeDuration);
		Time.timeScale = 1f;
		// *** SLOW MOTION ***

		//Hit particles.
		Instantiate (_hitParticles, _wolfArm.transform.position, Quaternion.identity);
		Instantiate (_hitPow, _wolfArm.transform.position, Quaternion.identity);

		SoundManager.Instance.PlayKillSound (_audio, "Bite");

		//optional delay before reactivating the game...
		if (_delay)
			yield return new WaitForSeconds (_delayDuration);

		//Release the player.
		_movement.enabled = true;

		//Switch back to sheep shape.
		_wolfShape.SetActive (false);
		_sheepShape.SetActive (true);
	}

	//If the victim is a NPSheep, only do the shape shift.
	public IEnumerator KillerFeedback ()
	{
		_sheepShape.SetActive (false);
		_wolfShape.SetActive (true);

		//SoundManager.Instance.PlayKillSound (_audio, "Poof");

		yield return new WaitForSeconds (_wolfShapeDuration);

		_wolfShape.SetActive (false);
		_sheepShape.SetActive (true);
	}

	public IEnumerator VictimFeedback(KillFeedback killer)
	{
		transform.LookAt (killer.transform.position);
		_vibration.KillVibration ();
		_movement.enabled = false;		//movement enabled when the player is respawned.

		//"Hide" the sheep when the slowmo is over...
		yield return new WaitForSeconds (_wolfShapeDuration);
		_sheepShape.SetActive (false);

		//(optional delay before reactivating the game)
		if(_delay)
			yield return new WaitForSeconds (_delayDuration);

		//... and make it visible again (it is respawned during the next frame).
		_sheepShape.SetActive (true);

		SoundManager.Instance.PlayKillSound (_audio, "Poof");

	}

	//former script
   // public GameObject VictimHitParticles;
//  public AudioClip Fail;
//  public AudioClip Success;
//	private AudioSource _audioSource;
   /* private bool coorutineIsRunning = false;

//    void Awake()
//    {
//        _audioSource = GetComponent<AudioSource>();
//    }

    void OnEnable()
    {
        Sheep.OnNpSheepWasKilled += PlayerKilledSheep;
        Player.OnPlayerKilled += PlayerKilledPlayer;
    }

    void OnDisable()
    {
        Sheep.OnNpSheepWasKilled -= PlayerKilledSheep;
        Player.OnPlayerKilled -= PlayerKilledPlayer;
    }

    void PlayerKilledPlayer(Player killer, Player victim)
    {
        if (!coorutineIsRunning)
            StartCoroutine(SuccessfullKillFeedback(killer, victim));
    }

    void PlayerKilledSheep(Player killer, Sheep sheep)
    {
        UnsuccessfulKillFeedback(killer);
    }

    IEnumerator SuccessfullKillFeedback(Player killer, Player victim)
    {
        coorutineIsRunning = true;

        killer.transform.FindChild("NameTag").gameObject.SetActive(true);
        victim.transform.FindChild("NameTag").gameObject.SetActive(true);

        //Stop the players
        var prevKillerSpeed = killer.speed;
        var prevVictimSpeed = victim.speed;
        killer.speed = 0;
        victim.speed = 0;

        //Attack Visuals
        killer.secondAnimationController.SetTrigger("Attack");
        killer.enabled = false;
        victim.enabled = false;

        Time.timeScale = .1f;
		SoundManager.Instance.PauseGameMusic ();

		SoundManager.Instance.PlaySuccessSound ();

        yield return new WaitForSeconds(killer.GetComponent<ShapeshiftAbility>().TimeSpentAsAWolf);


        killer.transform.FindChild("NameTag").gameObject.SetActive(false);
        victim.transform.FindChild("NameTag").gameObject.SetActive(false);

        killer.enabled = true;
        killer.speed = prevKillerSpeed;
        victim.enabled = true;
        victim.speed = prevVictimSpeed;
        victim.RespawnPlayer();
        
        Time.timeScale = 1f;
		SoundManager.Instance.UnpauseGameMusic ();
        coorutineIsRunning = false;
    }

    private void UnsuccessfulKillFeedback(Player killer)
    {
        killer.GetComponent<Player>().secondAnimationController.SetTrigger("Attack");
        var camShake = Camera.main.GetComponent<iTweenEvent>();
        if (camShake) Camera.main.GetComponent<iTweenEvent>().Play();
    }
    */
}
