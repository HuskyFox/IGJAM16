using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script used the generic ObjectPooler script to spawn the NPSheep
[RequireComponent(typeof (ObjectPooler))]
public class NPSheepSpawner : MonoBehaviour
{
	
	[SerializeField] private int _npSheepToSpawn = 20;	//how many NPSheep do we want ?
	[Range(0, 10)]
	[SerializeField] private float _spawnRadius = 5;	//how far from the center are they allowed to spawn ?
	[SerializeField] private ObjectPooler _objectPooler;

	public List <GameObject> npSheepInGame = new List <GameObject> ();

	void Start()
	{
		if (_npSheepToSpawn > _objectPooler.pooledAmount)
			_npSheepToSpawn = _objectPooler.pooledAmount;
	}

	//function called by the GameStateManager scripts when the game is ready.
	public void SpawnNPSheep()
	{
		//Sets the NPSheep inactive and clears the list.
		//(useful when the game restarts)
		RefreshPool ();

		for (int i = 0 ; i < _npSheepToSpawn ; i ++)
		{
			if (npSheepInGame.Count >= _npSheepToSpawn)
				return;

			//Takes an object from the pool...
			GameObject npSheep = _objectPooler.GetPooledObject ();

			if (npSheep == null)
				return;

			//sets it position to a random one inside a sphere...
			Vector3 spawnPosition = Random.insideUnitSphere * _spawnRadius;
			spawnPosition.y = 0;
			npSheep.transform.position = spawnPosition;

			//sets it ative and adds it to the list.
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
		}

		npSheepInGame.Clear ();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _spawnRadius);
	}
}