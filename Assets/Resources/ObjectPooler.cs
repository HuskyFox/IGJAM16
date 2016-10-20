using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : UnitySingleton <ObjectPooler> 
{
	public GameObject pooledObject;
	public int pooledAmount = 25;
	public bool poolCanGrow = true;

	List<GameObject> pooledObjects;

	void Start()
	{
		pooledObjects = new List<GameObject> ();

		for (int i = 0 ; i < pooledAmount ; i++)
		{
			GameObject obj = (GameObject)Instantiate (pooledObject);
			obj.transform.SetParent (transform);
			obj.SetActive (false);
			pooledObjects.Add (obj);
		}
	}

	public GameObject GetPooledObject()
	{
		for (int i = 0 ; i < pooledObjects.Count ; i++)
		{
			if(!pooledObjects[i].activeInHierarchy)
				return pooledObjects[i];
		}

		if (poolCanGrow)
		{
			GameObject obj = (GameObject) Instantiate(pooledObject);
			pooledObjects.Add(obj);
			return obj;
		}

		return null;
	}
}
