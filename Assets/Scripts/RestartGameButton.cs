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
        PlayerManager playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        for (int i=1;i<=playerManager.players.Count;i++)
        {
            GameObject.Find("Player_" + i).SendMessage("RespawnPlayer");

        }
        SoundManager.instance.StopRestartMusic();
        SoundManager.instance.PlayGameMusic();
        Application.LoadLevel(Application.loadedLevel);
	}
}
