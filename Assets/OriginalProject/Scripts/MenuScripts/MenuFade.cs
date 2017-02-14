using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuFade : MonoBehaviour
{

    public RawImage fadeImage;


    private float fadeMax = 0;
    private float currentFade = 1;



    // public float waitTime;

    void Awake()
    {

        FadeOut();

    }

    public void FadeIn()
    {
        fadeMax = 0;
        currentFade = 1;
        StartCoroutine(FIn());
    }

    public void FadeOut()
    {
        fadeMax = 1;
        currentFade = 0;
        StartCoroutine(FOut());
    }

    IEnumerator FIn()
    {


        while (currentFade >= fadeMax)
        {
            currentFade -= Time.deltaTime;
            fadeImage.material.SetFloat("_Cutoff", currentFade);
            yield return null;
        }
    }

    IEnumerator FOut()
    {


        while (currentFade <= fadeMax)
        {
            currentFade += Time.deltaTime;
            fadeImage.material.SetFloat("_Cutoff", currentFade);
            yield return null;
        }
    }
}