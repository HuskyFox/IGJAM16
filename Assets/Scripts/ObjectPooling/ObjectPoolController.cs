using UnityEngine;

namespace ObjectPooling
{
    [RequireComponent(typeof(ObjectPoolManager))]
    public class ObjectPoolController : MonoBehaviour {

        private static ObjectPoolController S_Singleton = null;

        public static ObjectPoolController Instance
        {
            get
            {

                if (S_Singleton == null)
                {
                    S_Singleton = new GameObject("ObjectPoolController").AddComponent<ObjectPoolController>();

                }
                return S_Singleton;

            }
        }

        private ObjectPoolManager _objectPoolManager;

        void Awake()
        {
            if (S_Singleton)
            {
                DestroyImmediate(gameObject); // delete duplicates (if any)
            }
            else {
                S_Singleton = this;
                //DontDestroyOnLoad(gameObject); //in order to preserve this object in new screen loads.
            }

            _objectPoolManager = GetComponent<ObjectPoolManager>();

        }

        public ObjectPool CreateNewObjectPool(GameObject objectToPool, int preloadedObjects = 3, bool poolCanGrow = true)
        {
            return _objectPoolManager.CreateNewPool(objectToPool, preloadedObjects, poolCanGrow);
        }

        public ObjectPool FindPool(string nameOfPooledObject)
        {
            return _objectPoolManager.FindPool(nameOfPooledObject);
        }

    }
}
