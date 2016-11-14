using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
	[SerializeField]
	SceneManagerUtils scene;

	void Awake()
	{
		string activeSceneName = SceneManager.GetActiveScene ().name;
		//if (activeSceneName != "Main Menu" || activeSceneName != "Game Scene")
		if (activeSceneName == "NeverUnload")
			scene.LoadScene ("Main Menu", null);
	}

	void OnEnable()
	{
		StartReturnButtons.OnGameIsReady += LoadGameScene;
		MenuFunctions.OnMainMenuPressed += LoadMenu;
	}

	public void LoadGameScene()
	{
		scene.LoadScene ("Game Scene", "Main Menu");
	}

	public void LoadMenu()
	{
		scene.LoadScene ("Main Menu", "Game Scene");
	}

	void OnDisable()
	{
		StartReturnButtons.OnGameIsReady -= LoadGameScene;
		MenuFunctions.OnMainMenuPressed -= LoadMenu;
	}
}
