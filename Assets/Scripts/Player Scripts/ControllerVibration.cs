using UnityEngine;
using System.Collections;
using InControl;

public class ControllerVibration : MonoBehaviour 
{
	[SerializeField] private float _wolfVibrationIntensity = 0.1f;
	private InputDevice _controller;
	private bool _isWolf = false;		//to stop and resume the vibrations when the game is paused/unpaused.
	private bool _gamePaused = false;
	private bool _isKilled = false;

	void Start()
	{
		_controller = GetComponent<PlayerData> ().controller;
	}

	void Update()
	{
		if (_isWolf)
			_controller.Vibrate (_wolfVibrationIntensity);
		else
			if(!_isKilled)_controller.StopVibration ();
	}

	/* Called by the PlayerData script.
	 * When the game is paused, vibrations stop.
	 * When the game is resumed, vibrations start again if the player was the wolf.*/
	public void PauseToggle ()
	{
		_gamePaused = !_gamePaused;

		if (_gamePaused)
			_controller.StopVibration ();
		else if (_isWolf)
			WolfVibration ();
	}
		
	public void WolfVibration()
	{
		//_controller.Vibrate (_wolfVibrationIntensity);
		_isWolf = true;
	}

	public void Stop()
	{
		//_controller.StopVibration ();
		_isWolf = false;
	}

	//A flash vibration to let the player know she was killed.
	public void KillVibration()
	{
		_isKilled = true;
		StartCoroutine (Vibration ());
	}

	IEnumerator Vibration ()
	{
		float start = Time.realtimeSinceStartup;

		_controller.Vibrate (0.2f, 0.2f);

		while (Time.realtimeSinceStartup < start + 0.3f)
		{
			yield return null;
		}

		_controller.StopVibration ();
		_isKilled = false;
	}

	void OnDisable()
	{
		_controller.StopVibration ();
	}

}
