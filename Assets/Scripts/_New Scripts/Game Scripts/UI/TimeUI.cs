using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
	[SerializeField] Text timerLabel;
	[SerializeField] GameObject wolfCountdownDisplay;
	[SerializeField] Text wolfCountdown;
	[SerializeField] GameObject countdownDisplay;
	[SerializeField] Text countdown;

	[HideInInspector]public float gameTimeLeft;
	float mins;
	float seconds;
	Animation timerAnim;
	bool animPlaying = false;
	[HideInInspector]public float wolfSeconds;
	[HideInInspector] public float countdownSeconds;

	void Awake()
	{
		timerAnim = timerLabel.GetComponent<Animation> ();
	}

	void OnEnable()
	{
		NewWolfManager.OnTimeForANewWolf += ShowWolfCountdown;
	}

	void Update()
	{
		//countdown.text = countdownSeconds.ToString ("F0");

//		mins = Mathf.Floor(gameTimeLeft / 60);
//		seconds = Mathf.Floor (gameTimeLeft % 60);
//		timerLabel.text = string.Format ("{0:0}:{1:00}", mins, seconds);

		if(gameTimeLeft + 0.25f == 5.25f && gameTimeLeft > 0)
		if(!animPlaying)
			StartCoroutine (TimerAnimation ());

		wolfCountdown.text = wolfSeconds.ToString ("F0");
	}

	IEnumerator TimerAnimation()
	{
		animPlaying = true;
		timerAnim.Play ();
		yield return new WaitForSeconds (6f);
		timerAnim.Stop ();
		animPlaying = false;
	}

	public void Countdown()
	{
		countdownDisplay.SetActive (!countdownDisplay.activeSelf);
	}

	void ShowWolfCountdown(float countdown)
	{
		wolfCountdownDisplay.SetActive (true);
		Invoke ("HideWolfCountdown", countdown);
	}

	public void HideWolfCountdown()
	{
		wolfCountdownDisplay.SetActive (false);	
	}

	void OnDisable()
	{
		NewWolfManager.OnTimeForANewWolf -= ShowWolfCountdown;
	}
}
