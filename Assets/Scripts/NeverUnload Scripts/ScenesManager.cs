using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/* This script loads the menu and game scene by calling functions written in the SceneManagerUtils script.
 * Depending on what the active scene when play is hit in the Unity Editor, it detects if we are playtesting in the game scene.
 * If so, it instantiates a gameobject that takes care of the controller registration (instead of the main menu).*/
public class ScenesManager : MonoBehaviour
{
	[SerializeField] private SceneManagerUtils _scene;
	#if(UNITY_EDITOR)
	[SerializeField] private GameObject _playtest;
	#endif

	void Awake()
	{
		// loads the main menu scene if there is no other scene than the NeverUnload.
		string activeSceneName = SceneManager.GetActiveScene ().name;
		if (activeSceneName == "NeverUnload") 
		{
			_scene.LoadScene ("Main Menu", null);
			SoundManager.Instance.PlayMusic ("Menu");
		}
		//Are we playtesting in the game scene?
		#if (UNITY_EDITOR)
		else if (activeSceneName == "Game Scene")
		{
			print("Playtesting...");
			GameObject playtest = Instantiate (_playtest) as GameObject;	//enable the alternate controller registration
		}
		#endif
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
		SoundManager.Instance.PlayMusic ("Menu");
	}

	void OnDisable()
	{
		StartReturnButtons.OnGameIsReady -= LoadGameScene;
		MenuFunctions.OnMainMenuPressed -= LoadMenu;
	}
}
