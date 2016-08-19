using UnityEngine;
using System.Collections;

public class KillFeedback : MonoBehaviour
{
    public AudioClip Fail;
    public AudioClip Success;
   	private bool coorutineIsRunning = false;
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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
		{
            StartCoroutine(SuccessfullKillFeedback(killer));
//			for (int i = 0; i <= SoundManager.instance.sounds.Length; i++)
//			{
//				SoundManager.instance.sounds [i].pitch = .5f;
//			}
		}
    }

    void PlayerKilledSheep(Player killer, Sheep sheep)
    {
        UnsuccessfulKillFeedback();
    }

    IEnumerator SuccessfullKillFeedback(Player killer)
    {

        coorutineIsRunning = true;
//        _audioSource.clip = Success;
//        _audioSource.Play();
        Time.timeScale = .1f;
        var prevSpeed = killer.speed;
        killer.speed = 0;

        yield return new WaitForSeconds(killer.GetComponent<ShapeshiftAbility>().TimeSpentAsAWolf);
        
        killer.speed = prevSpeed;
        Time.timeScale = 1f;
        coorutineIsRunning = false;
//				for (int i = 0; i <= SoundManager.instance.sounds.Length; i++)
//				{
//					SoundManager.instance.sounds [i].pitch = 1f;
//				}
    }



    private void UnsuccessfulKillFeedback()
    {
        var camShake = Camera.main.GetComponent<iTweenEvent>();
        if (camShake) Camera.main.GetComponent<iTweenEvent>().Play();
//        _audioSource.clip = Fail;
//        _audioSource.Play();
    }
}
