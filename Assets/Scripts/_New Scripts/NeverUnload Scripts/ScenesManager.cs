using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// This script loads the menu and game scene by calling functions written in the SceneManagerUtils script.
public class ScenesManager : MonoBehaviour
{
	[SerializeField] private SceneManagerUtils _scene;

	void Awake()
	{
		// loads the main menu scene if there is no other scene than the NeverUnload.
		string activeSceneName = SceneManager.GetActiveScene ().name;
		if (activeSceneName == "NeverUnload")
			_scene.LoadScene ("Main Menu", null);
	}

	void OnEnable()
	{
		StartReturnButtons.OnGameIsReady += LoadGameScene;
		MenuFunctions.OnMainMenuPressed += LoadMenu;
	}

	public void LoadGameScene()
	{
		_scene.LoadScene ("Game Scene", "Main Menu");
	}

	public void LoadMenu()
	{
		_scene.LoadScene ("Main Menu", "Game Scene");
	}

	void OnDisable()
	{
		StartReturnButtons.OnGameIsReady -= LoadGameScene;
		MenuFunctions.OnMainMenuPressed -= LoadMenu;
	}
}
