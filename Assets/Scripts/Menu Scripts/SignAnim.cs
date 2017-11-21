using UnityEngine;
using System.Collections;

public class SignAnim : MonoBehaviour 
{
	private Animation anim;

	void Start ()
	{
		anim = GetComponent<Animation> ();
		anim.PlayQueued ("SignAnim");
	}

}
