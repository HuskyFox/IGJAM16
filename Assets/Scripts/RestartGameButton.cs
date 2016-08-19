using UnityEngine;
using System.Collections;

public class RestartGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartGameScene() {
		Application.LoadLevel(Application.loadedLevel);
	}
}
