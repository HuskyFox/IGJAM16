using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject wolfTimerObject;
    public bool wolfTimer;
    public bool wolfCountdown;
    float wolfTime = 5;
    float wolfSeconds;
    public Text wolfTimerLabel;

    public WolfManager wolfMan;
    public bool gameFinished;
    public GameObject gameOverUI;
    public GameObject winnerIsUI;

    public Text timerLabel;
    public bool currentlyTiming;
    public float startingTime; //In seconds
    public float time;
    public float mins;
    public float seconds;
    float fraction;

    void Start()
    {
        time = startingTime;
		StartCoroutine(AmbianceRandomSheepSounds(Random.Range (1.3f, 5f)));
    }

	IEnumerator AmbianceRandomSheepSounds(float delay)
	{
		while (!gameFinished)
		{
			SoundManager.instance.PlayRandomSheepBaa ();
			delay = Random.Range (1.3f, 6f);
			print (delay);
			yield return new WaitForSeconds (delay);
		}

	}
    void Update()
    {
        if (!wolfTimer)
        {
            timerLabel.text = string.Format("{0:0}:{1:00}", mins, seconds);

            if (currentlyTiming == true)
            {
                time -= Time.deltaTime;
                mins = Mathf.Floor(time / 60);
                seconds = Mathf.Floor(time % 60); //Use the euclidean division for the seconds.

                if (time <= 0)
                {
                    GameOver();
                    currentlyTiming = false;
                    time = 0;
                    mins = 0;
                    seconds = 0;
                }

            }
        }

        if (wolfTimer)
        {
           // wolfTimerObject.SetActive(true);
            wolfTimerLabel.text = wolfSeconds.ToString();
            if (wolfCountdown == true)
            {
                wolfTime -= Time.deltaTime;
                wolfSeconds = Mathf.Floor(wolfTime % 60);

                if (wolfTime <= 0)
                {
                    print("End of Wolf Time");
                    SetNewWolf();
                    wolfTime = 5;
                    wolfSeconds = 5;
                    wolfCountdown = false;
                }
                
            }
        }

       /* if (gameOverUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("GameOverAnim"))
        {
            //do nothing
        }

        else
        {
            if (gameFinished == true)
            {
                winnerIsUI.SetActive(true);
                gameOverUI.SetActive(false);
            }
        }
        */

    }

    public void GameOver()
    {
		SoundManager.instance.StopGameMusic ();
		SoundManager.instance.PlayRestartMusic ();
        gameFinished = true;
        print("Time is up!");
        gameOverUI.SetActive(true);
        if (gameOverUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("GameOverAnim"))
        {
           //do nothing
        }

    }


    public void StartNewWolfCountdown()
    {
        this.transform.parent.gameObject.transform.localPosition = new Vector3(0, 178, 0);
        wolfCountdown = true;
        wolfTime = 5;
        wolfSeconds = 5;
        Debug.Log(wolfCountdown);
        Invoke("SetNewWolf", 5f);

    }

    public void SetNewWolf()
    {
        this.transform.parent.gameObject.transform.localPosition = new Vector3(0, 350, 0);
    }
}