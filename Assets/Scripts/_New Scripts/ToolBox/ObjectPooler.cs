using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This is a generic ObjectPooler script, that is used for the NPSheep.
 * I changed a bit the original script, leaving only the useful parts.*/
public class ObjectPooler : MonoBehaviour
{
	[SerializeField] private GameObject _pooledObject;
	[SerializeField] public int pooledAmount = 25;
	[SerializeField] private bool _poolCanGrow = true;

	private List<GameObject> _pooledObjects;

	void Awake()
	{
		_pooledObjects = new List<GameObject> ();

		/* Instantiates as many object as required,
		 * sets their parent and sets them inactive,
		 * adds them to the list of pooled objects.*/
		for (int i = 0 ; i < pooledAmount ; i++)
		{
			GameObject obj = (GameObject)Instantiate (_pooledObject);
			obj.transform.SetParent (transform);
			obj.SetActive (false);
			_pooledObjects.Add (obj);
		}
	}

	//this function is called by the manager script that handles the spawning of the pooled objects (currently NPSheepSpawner).
	public GameObject GetPooledObject()
	{
		//returns a still inactive gameobject from the pool, until there is no more.
		for (int i = 0 ; i < _pooledObjects.Count ; i++)
		{
			if(!_pooledObjects[i].activeInHierarchy)
				return _pooledObjects[i];
		}

		//If we can have more than the pooled amount, instantiates new objects.
		if (_poolCanGrow)
		{
			GameObject obj = (GameObject) Instantiate(_pooledObject);
			_pooledObjects.Add(obj);
			return obj;
		}

		return null;
	}
}
