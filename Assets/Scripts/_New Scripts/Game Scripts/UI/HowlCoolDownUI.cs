using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowlCoolDownUI : MonoBehaviour
{
	private Image howlImage;
	[HideInInspector]public float coolDownTime;

	void OnEnable()
	{
		howlImage = GetComponent<Image> ();
		howlImage.fillAmount = 0f;
		print ("Starting cooling down : " + coolDownTime);
	}

	void Update()
	{
		howlImage.fillAmount += (1.0f / coolDownTime) * Time.deltaTime;

		if(howlImage.fillAmount == 1f)
			this.enabled = false;
	}

}

