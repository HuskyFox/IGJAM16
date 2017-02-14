using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// The EventSystem is in the NeverUnload scene, and it moves itself to the active scene (be it the Main Menu or the Game Scene if playtesting).
public class MoveToActiveScene : MonoBehaviour
{
	void Awake()
	{
		Scene activeScene = SceneManager.GetActiveScene ();
		SceneManager.MoveGameObjectToScene (gameObject, activeScene);
	}
}
