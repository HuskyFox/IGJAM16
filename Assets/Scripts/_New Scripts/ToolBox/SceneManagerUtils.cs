using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class SceneManagerUtils : MonoBehaviour
{
	public bool sceneLoaded = false;

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

	public void LoadSceneWithPersist(string currentScene, string sceneToLoad, List<GameObject> objectsToPersist)
	{	
		sceneLoaded = false;
		StartCoroutine (ChangeSceneWithPersist (currentScene, sceneToLoad, objectsToPersist));
	}

	IEnumerator ChangeSceneWithPersist (string currentScene, string sceneToLoad, List<GameObject> objectsToPersist)
	{/*
		AsyncOperation ao = SceneManager.LoadSceneAsync (sceneToLoad, LoadSceneMode.Additive);
		ao.allowSceneActivation = false;

		while (!ao.isDone)
		{
			if(ao.progress == 0.9f)
			{
				for (int i = 0 ; i < objectsToPersist.Count ; i++)
				{
					SceneManager.MoveGameObjectToScene (objectsToPersist [i], SceneManager.GetSceneByName (sceneToLoad));
				}

				//if(InputManager.ActiveDevice.Action1.WasPressed)
					ao.allowSceneActivation = true;
			}

			yield return null;
		}
		sceneLoaded = true;
		SceneManager.UnloadScene (currentScene);
		*/
		
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
	}




    public void QuitGame()
    {
        Application.Quit();
    }
}