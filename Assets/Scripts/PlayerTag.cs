using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerTag : MonoBehaviour
{

    public List<Color> Colors = new List<Color>();

	// Use this for initialization
	void Start ()
	{
        Colors.Add(Color.black);
        Colors.Add(Color.blue);
        Colors.Add(Color.red);
        Colors.Add(Color.green);
        Colors.Add(Color.cyan);
	    string pindex = transform.root.GetComponent<Player>().playerIndex;
        GetComponent<Text>().text = "P" + pindex;
	    int playerIndex = 0;
	    int.TryParse(pindex, out playerIndex);
	    GetComponent<Text>().color = Colors[playerIndex];
	}

    void OnEnable()
    {
        Colors.Add(Color.black);
        Colors.Add(Color.blue);
        Colors.Add(Color.red);
        Colors.Add(Color.green);
        Colors.Add(Color.cyan);
        string pindex = transform.root.GetComponent<Player>().playerIndex;
        GetComponent<Text>().text = "Player " + pindex;
        int playerIndex = 0;
        int.TryParse(pindex, out playerIndex);
        GetComponent<Text>().color = Colors[playerIndex];
    }
	
	// Update is called once per frame
	void Update () {
	    transform.LookAt(Camera.main.transform);
	}
}
