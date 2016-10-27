using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveToActiveScene : MonoBehaviour
{
	void Awake()
	{
		Scene activeScene = SceneManager.GetActiveScene ();
		SceneManager.MoveGameObjectToScene (gameObject, activeScene);
	}
}
