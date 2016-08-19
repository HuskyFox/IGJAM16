using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerUtils : MonoBehaviour
{

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

    public void QuitGame()
    {
        Application.Quit();
    }
}