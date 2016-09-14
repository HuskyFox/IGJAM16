using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPool : MonoBehaviour
    {

        public int ObjectsPreLoaded;
        public bool PoolCanGrow = true;
        public GameObject ObjectToPool;

		private List<GameObject> _objectPool = new List<GameObject>();
        private bool _poolIsLoaded = false;

        void Start()
        {
            LoadPool();
        }

        public void LoadPool()
        {
            if (_poolIsLoaded)
                return;
            _objectPool = new List<GameObject>();
            for (var i = 0; i < ObjectsPreLoaded; i++)
            {
                GameObject tempObject = Instantiate(ObjectToPool); //instantiate the object
                tempObject.SetActive(false);
                tempObject.transform.SetParent(transform);
				_objectPool.Add(tempObject);
            }

            _poolIsLoaded = true;

        }

        public GameObject GetPooledObject()
        {

            foreach (var objectpool in _objectPool)
            {
                if (!objectpool.activeInHierarchy)
                {
                    return objectpool;

                }
            }

            if (PoolCanGrow)
            {
                GameObject tempObject = Instantiate(ObjectToPool);
                tempObject.transform.SetParent(transform);
                _objectPool.Add(tempObject);
                return tempObject;

            }

            return null; //if pool is not allowed to grow and there is no objects to get.
        }

    }
}
