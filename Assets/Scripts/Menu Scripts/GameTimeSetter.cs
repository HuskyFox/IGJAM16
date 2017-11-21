using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

//Let the player choose the game duration.
public class GameTimeSetter : MonoBehaviour
{
	[SerializeField] private Text _duration;
	[SerializeField] private Text _minus;
	[SerializeField] private Text _plus;
	private int current = 1;

	public delegate void GameDurationModified (int duration);
	public static event GameDurationModified OnGameDurationModified;
	
	void Update () 
	{
		if (InputManager.ActiveDevice.LeftBumper.WasPressed)
		{
			if(current > 0)
			{
				current -= 1;
				ChangeDuration ();
			}
		}
		else if (InputManager.ActiveDevice.RightBumper.WasPressed)
		{
			if(current < 4)
			{
				current += 1;
				ChangeDuration ();
			}
		}
	}

	void ChangeDuration()
	{
		switch (current)
		{
		case 0:
			_minus.color = Color.grey;
			_duration.text = "2:00";
			break;
		case 1:
			_minus.color = Color.white;
			_duration.text = "2:30";
			break;
		case 2:
			_duration.text = "3:00";
			break;
		case 3:
			_plus.color = Color.white;
			_duration.text = "4:00";
			break;
		case 4:
			_plus.color = Color.grey;
			_duration.text = "5:00";
			break;
		}
		if (OnGameDurationModified != null)
			OnGameDurationModified (current);	//listened to by the GameData script.
	}
}
