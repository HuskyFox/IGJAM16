using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowlCoolDownUI : MonoBehaviour
{
	private Image howlImage;
	public float coolDownTime {get; set; }
	//private bool coolingDown;

	void OnEnable()
	{
		howlImage = GetComponent<Image> ();
		//coolingDown = true;
		howlImage.fillAmount = 0f;
		print ("Starting cooling down : " + coolDownTime);
	}

	void Update()
	{
//		do 
//		{
//			howlImage.fillAmount += (1.0f / coolDownTime) * Time.deltaTime;
//		} while (howlImage.fillAmount != 1f);

		howlImage.fillAmount += (1.0f / coolDownTime) * Time.deltaTime;
		if(howlImage.fillAmount == 1f)
		{
			print ("Cooling down end!");
			this.enabled = false;
		}

	}

//	public void CoolDown ()
//	{
//		while (howlImage.fillAmount != 1f)
//		{
//			howlImage.fillAmount += 1.0f / coolDownTime * Time.deltaTime;
//		}
//		coolingDown = false;
//		print ("Cooling down end!");
//	}
}

