using UnityEngine;
using System.Collections;

public class KillFeedback : MonoBehaviour
{	
	public float timeSpentInWolfShape = 1f;
	public Animator wolfAnimator;
	public GameObject wolfShape;
	public GameObject sheepShape;
	public GameObject wolfArm;
	public GameObject killerHitParticles;
	private bool shapeShiftCoroutineIsRunning = false;
	private bool slowMoCoroutineIsRunning = false;

	//switch the shape from sheep to wolf, and then back to sheep.
	public void ShapeShiftFeedback (PlayerController killer)
	{
		sheepShape.SetActive (false);
		wolfShape.SetActive (true);
		if (!shapeShiftCoroutineIsRunning)
			StartCoroutine (ShapeShift (killer));
	}

	IEnumerator ShapeShift (PlayerController killer)
	{
		shapeShiftCoroutineIsRunning = true;

		yield return new WaitForSeconds (timeSpentInWolfShape);

		wolfShape.SetActive (false);
		sheepShape.SetActive (true);
		shapeShiftCoroutineIsRunning = false;
	}

	//calls the coroutine that slows time when a success kill happens
	public void SlowMoFeedback (PlayerController killer, PlayerController victim)
	{
		if (!slowMoCoroutineIsRunning)
			StartCoroutine(SlowMo(killer, victim));
	}

	IEnumerator SlowMo(PlayerController killer, PlayerController victim)
	{
		slowMoCoroutineIsRunning = true;

		//killer.transform.FindChild("NameTag").gameObject.SetActive(true);
		//victim.transform.FindChild("NameTag").gameObject.SetActive(true);

		//Stop the players
		var prevKillerSpeed = killer.speed;
		var prevVictimSpeed = victim.speed;
		killer.speed = 0;
		victim.speed = 0;

		//the wolf looks at the victim
		killer.transform.LookAt (victim.transform.position);

		//Attack Visuals
		wolfAnimator.SetTrigger("Attack");
		killer.enabled = false;
		victim.enabled = false;

		//slows time
		Time.timeScale = .1f;

		//SoundManager.Instance.PauseGameMusic ();

		//SoundManager.Instance.PlaySuccessSound ();

		yield return new WaitForSeconds(timeSpentInWolfShape);


		//killer.transform.FindChild("NameTag").gameObject.SetActive(false);
		//victim.transform.FindChild("NameTag").gameObject.SetActive(false);
		Instantiate (killerHitParticles, wolfArm.transform.position, Quaternion.identity);

		killer.enabled = true;
		killer.speed = prevKillerSpeed;
		victim.enabled = true;
		victim.speed = prevVictimSpeed;

		victim.RespawnPlayer();

		//speeds back time
		Time.timeScale = 1f;
		//SoundManager.Instance.UnpauseGameMusic ();
		slowMoCoroutineIsRunning = false;
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
