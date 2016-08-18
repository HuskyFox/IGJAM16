using UnityEngine;
using System.Collections;

public class MenuSheep : MonoBehaviour {

    public GameObject menuSheep;
    public bool inverse;

	// Use this for initialization
	void Start () {
        Invoke("SpawnSheep", Random.Range(3, 6));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SpawnSheep()
    {
        if (!inverse)
        {
            GameObject sheep = (GameObject)Instantiate(menuSheep, this.transform.position, Quaternion.identity);
            sheep.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(30, 70), Random.Range(-30, 30), 0));
            sheep.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), Random.Range(-25, 25)));
            Destroy(sheep, 20f);
        }

        else
        {
            GameObject sheep = (GameObject)Instantiate(menuSheep, this.transform.position, Quaternion.identity);
            sheep.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-70, -30), Random.Range(30, -30), 0));
            sheep.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), Random.Range(-25, 25)));
            Destroy(sheep, 20f);
        }


        Start();
    }
}
