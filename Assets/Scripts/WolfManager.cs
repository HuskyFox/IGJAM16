using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WolfManager : MonoBehaviour 
{
	public float minTimeBetweenWolfSwitch = 10f;
	public float maxTimeBetweenWolfSwitch = 30f;

	float timer;

//	void Start()
//	{
//		GenerateRandomTimeBetweenSwitch ();
//	}

	void Update()
	{
		timer += Time.deltaTime;
		float timeBetweenSwitch = Random.Range (minTimeBetweenWolfSwitch, maxTimeBetweenWolfSwitch);

		if (timer >= timeBetweenSwitch)
			WolfSwitch ();
	}

	void WolfSwitch()
	{
		timer = 0f;

	}

//	void GenerateRandomTimeBetweenSwitch()
//	{
//		float timeBetweenSwitch = Random.Range (minTimeBetweenWolfSwitch, maxTimeBetweenWolfSwitch);
//		Debug.Log (timeBetweenSwitch);
//	}
}
