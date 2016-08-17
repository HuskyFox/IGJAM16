using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPoolManager : MonoBehaviour
    {

        public GameObject ObjectToPool;
        [Range(0, 50)]
        public int ObjectsPreloaded = 1;
        public bool PoolCanGrow = false;

        private List<ObjectPool> _objectPools = new List<ObjectPool>();

        void Awake()
        {
            RefreshPoolList();
        }

        public void RefreshPoolList()
        {
            _objectPools = new List<ObjectPool>();
            foreach (Transform child in transform)
            {
                var goPool = child.gameObject.GetComponent<ObjectPool>();
                if (goPool)
                    _objectPools.Add(goPool);
            }
        }

        public ObjectPool CreateNewPool(GameObject objectToPool, int objectsPreloaded = 3, bool poolCanGrow = true)
        {
            if (!objectToPool)
                return null;
            if (PoolExists(objectToPool))
                return FindPool(objectToPool.name);

            //Create an empty gameObject to categorize the new pool.
            var poolParent = new GameObject(objectToPool.name);
            poolParent.transform.position = transform.position;
            poolParent.transform.SetParent(transform);

            //Give the pool the objectPool component
            var poolComponent = poolParent.AddComponent<ObjectPool>();
            poolComponent.ObjectToPool = objectToPool;
            poolComponent.ObjectsPreLoaded = objectsPreloaded;
            poolComponent.PoolCanGrow = poolCanGrow;

            _objectPools.Add(poolComponent);

            return poolComponent;

        }

        public ObjectPool FindPool(string pooledObjectName)
        {
            return _objectPools.FirstOrDefault(objectPool => objectPool.ObjectToPool.name.Equals(pooledObjectName));
        }

        public bool PoolExists(GameObject objectbeingPooled)
        {
            return _objectPools.Any(objectPool => objectPool.ObjectToPool.name.Equals(objectbeingPooled.name));
        }
    }
}

