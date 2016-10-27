using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPSheepSpawner : MonoBehaviour
{
	
	public int npSheepToSpawn = 20;
	[Range(0, 10)]
	public float spawnRadius = 5;

	[SerializeField]
	ObjectPooler objectPooler;

	public List <GameObject> npSheepInGame;

	void Start()
	{
		npSheepInGame = new List<GameObject>();
		if (npSheepToSpawn > objectPooler.pooledAmount)
			npSheepToSpawn = objectPooler.pooledAmount;
	}

	public void SpawnNPSheep()
	{
		RefreshPool ();

		for (int i = 0 ; i < npSheepToSpawn ; i ++)
		{
			if (npSheepInGame.Count >= npSheepToSpawn)
				return;
			
			GameObject npSheep = objectPooler.GetPooledObject ();

			if (npSheep == null)
				return;

			Vector3 spawnPosition = Random.insideUnitSphere * spawnRadius;
			spawnPosition.y = 0;

			npSheep.transform.position = spawnPosition;
			npSheep.SetActive(true);

			npSheepInGame.Add (npSheep);
		}
	}

	void RefreshPool()
	{
		if (npSheepInGame.Count == 0)
			return;
		
		for (int i = 0 ; i < npSheepInGame.Count ; i++)
		{
			npSheepInGame [i].SetActive (false);
			npSheepInGame.RemoveAt (i);
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, spawnRadius);
	}
}
