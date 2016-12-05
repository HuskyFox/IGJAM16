using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* This script is in charge of the cooldown bar for the wolf howl.
 * It is handled by the HowlManager.*/
public class HowlCoolDownUI : MonoBehaviour
{
	[HideInInspector] public bool currentlyPlaying = false;
	private Image _cooldownBar;
	private float _step;

	void Awake()
	{
		_cooldownBar = GetComponent<Image> ();
	}

	//Reset the bar when a new wolf is chosen.
	public void ResetCooldown()
	{
		_cooldownBar.fillAmount = 1f;
	}

	//Empty the bar and start the fill up.
	public IEnumerator FillUp(float cooldown)
	{
		currentlyPlaying = true;		//make sure that it will fill up.
		_cooldownBar.fillAmount = 0f;	//empty the bar.
		_step = (1f / cooldown);		//calculate step.

		//Fill up the bar
		while (_cooldownBar.fillAmount != 1f)
		{
			//Stop the fill up if the game is paused.
			if (!currentlyPlaying)
				yield return StartCoroutine (WaitForUnpause ());
			
			_cooldownBar.fillAmount += _step * Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator WaitForUnpause()
	{
		while (!currentlyPlaying)
		{
			yield return null;
		}
	}

}

