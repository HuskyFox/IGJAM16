using UnityEngine;
using System.Collections;
using InControl;

public class ControllerVibration : MonoBehaviour 
{
	private InputDevice _controller;
	private bool _isWolf = false;
	private bool _gamePaused = false;

	void Start()
	{
		_controller = GetComponent<PlayerData> ().controller;
	}

	public void PauseToggle ()
	{
		_gamePaused = !_gamePaused;

		if (_gamePaused)
			_controller.StopVibration ();
		else if (_isWolf)
			WolfVibration ();
	}

	public void Unpause()
	{
		if (_isWolf)
			WolfVibration ();
	}

	public void WolfVibration()
	{
		_controller.Vibrate (0.1f);
		_isWolf = true;
	}

	public void Stop()
	{
		_controller.StopVibration ();
		_isWolf = false;
	}

	public void KillVibration()
	{
		StartCoroutine (Vibration (0.3f));
	}

	IEnumerator Vibration (float duration)
	{
		float start = Time.realtimeSinceStartup;

		_controller.Vibrate (0.2f, 0.2f);

		while (Time.realtimeSinceStartup < start + duration)
		{
			yield return null;
		}

		_controller.StopVibration ();
	}

	void OnDisable()
	{
		_controller.StopVibration ();
	}

}
