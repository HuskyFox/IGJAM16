using UnityEngine;
using System.Collections;

public class NPSheep : MonoBehaviour 
{
	public GameObject killParticles;

	public void TakeDamage (NPSheep victim)
	{
		var particles = Instantiate(killParticles);
		if (particles) particles.transform.position = transform.position;
		gameObject.SetActive (false);
	}

	public void CamShake()
	{
		//killer.GetComponent<Player>().secondAnimationController.SetTrigger("Attack");
		var camShake = Camera.main.GetComponent<iTweenEvent>();
		if (camShake) Camera.main.GetComponent<iTweenEvent>().Play();
	}

}
