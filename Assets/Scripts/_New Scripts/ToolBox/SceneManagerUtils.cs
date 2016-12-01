using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using InControl;

/* This script contains several scene management functions.
 * We are currently using the first one, which loads the next scene additively and unloads the current one.
 * This trick is necessary to be able to keep the NeverUnload scene at all times.
 * (The Unity documentation advises to avoid using DontDestroyOnLoad anymore,
 * (so I've had to come up with a solution, and after many research that's the best one I found.
 * (There might be another way, though ;) ).
 */
public class SceneManagerUtils : MonoBehaviour
{
	//allows to pass the EventSystem from one scene to another.
	[SerializeField] private EventSystem _eventSystem;

	//[HideInInspector] public bool sceneLoaded = false;

	public void LoadScene(string sceneToLoad, string currentSceneName)
	{
		StartCoroutine (LoadSceneAndSetActive (sceneToLoad, currentSceneName));
	}

	IEnumerator LoadSceneAndSetActive(string sceneName, string currentSceneName)
	{
		//Loads the new scee additively and async, to keep track of the loading process.
		var loading = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		while (!loading.isDone)
		{
			yield return null;
		}

		//Once the scene is loaded, we move the EventSystem to the loaded scene...
		Scene scene = SceneManager.GetSceneByName (sceneName);
		SceneManager.MoveGameObjectToScene (_eventSystem.gameObject, scene);
		//then set it active...
		SceneManager.SetActiveScene (scene);
		//and finally unload the previous scene.
		SceneManager.UnloadScene (currentSceneName);
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

    public void LoadNextScene()
    {
        var currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneBuildIndex = currentSceneBuildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneBuildIndex)
            SceneManager.LoadScene(nextSceneBuildIndex);
    }

    public void LoadPreviousScene()
    {
        var currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        var previousSceneBuildIndex = currentSceneBuildIndex - 1;
        if (previousSceneBuildIndex >= 0)
            SceneManager.LoadScene(previousSceneBuildIndex);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	/* This was my first successful iteration over the problem.
	 * It required to use a transition scene for the passing of objects that needed to be kept.
	 * I didn't delete it because it might be useful one day.
	 * 
	public void LoadSceneWithPersist(string currentScene, string sceneToLoad, List<GameObject> objectsToPersist)
	{	
		sceneLoaded = false;
		StartCoroutine (ChangeSceneWithPersist (currentScene, sceneToLoad, objectsToPersist));
	}

	IEnumerator ChangeSceneWithPersist (string currentScene, string sceneToLoad, List<GameObject> objectsToPersist)
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync ("Transition", LoadSceneMode.Additive);
		ao.allowSceneActivation = false;

		while (!ao.isDone)
		{
			if(ao.progress == 0.9f)
			{
				for (int i = 0 ; i < objectsToPersist.Count ; i++)
				{
					SceneManager.MoveGameObjectToScene (objectsToPersist [i], SceneManager.GetSceneByName ("Transition"));
				}

				//if(InputManager.ActiveDevice.Action1.WasPressed)
				ao.allowSceneActivation = true;
			}

			yield return null;
		}
		SceneManager.UnloadScene (currentScene);

		AsyncOperation ao2 = SceneManager.LoadSceneAsync (sceneToLoad, LoadSceneMode.Additive);
		ao2.allowSceneActivation = false;

		while (!ao2.isDone)
		{
			if(ao2.progress == 0.9f)
			{
				for (int i = 0 ; i < objectsToPersist.Count ; i++)
				{
					SceneManager.MoveGameObjectToScene (objectsToPersist [i], SceneManager.GetSceneByName (sceneToLoad));
				}

				//if(InputManager.ActiveDevice.Action1.WasPressed)
				ao2.allowSceneActivation = true;
			}

			yield return null;
		}
		sceneLoaded = true;
		SceneManager.UnloadScene ("Transition");
	}*/

    public void QuitGame()
    {
        Application.Quit();
    }
}