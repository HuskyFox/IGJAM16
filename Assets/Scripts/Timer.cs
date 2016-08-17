using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

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

    void GameOver()
    {
        print("Time is up!");
    }
}