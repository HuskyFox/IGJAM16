using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public bool wolfTimer;
    public bool wolfCountdown;
    float wolfTime;
    float wolfSeconds;



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
            timerLabel.text = wolfSeconds.ToString();
            if (wolfCountdown == true)
            {
                wolfTime -= Time.deltaTime;
                wolfSeconds = Mathf.Floor(wolfTime % 60);

                if (wolfTime <= 0)
                {
                    SetNewWolf();
                    wolfTime = 0;
                    wolfSeconds = 0;
                    wolfCountdown = false;
                }
            }
        }


    }

    public void GameOver()
    {
        print("Time is up!");
    }

    public void SetNewWolf()
    {

    }
}