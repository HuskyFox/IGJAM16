using UnityEngine;
using System.Collections;

public class PauseUI : MonoBehaviour
{
	[SerializeField]
	GameObject pauseUI;

	public void DisplayPauseMenu()
	{
		pauseUI.SetActive (true);
	}

	public void HidePauseMenu()
	{
		pauseUI.SetActive (false);
	}

}
