using UnityEngine;
using System.Collections;
using InControl;

public class DemoMode : MonoBehaviour
{
	[SerializeField] private GameObject _instructions;

	void Update () 
	{
		if(InputManager.ActiveDevice.LeftBumper.IsPressed)
		{
			if (InputManager.ActiveDevice.RightBumper.WasPressed)
				_instructions.SetActive (!_instructions.activeSelf);
		}

		if(Input.GetKey (KeyCode.A))
		{
			if (Input.GetKeyDown (KeyCode.Z))
				_instructions.SetActive (!_instructions.activeSelf);
		}
	}
}
