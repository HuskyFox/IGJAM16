using ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour {

    [Range(5, 100)]
    public float SpawnRadius = 10f;

    public float SpawnRate = 1f;    //Seconds to wait per spawn

    public bool StartActive;
    public GameObject ObjectToSpawn;

    private ObjectPool _objectPool;

    void Start()
    {
        _objectPool = ObjectPoolController.Instance.FindPool(ObjectToSpawn.name);
        if (!_objectPool)
            _objectPool = ObjectPoolController.Instance.CreateNewObjectPool(ObjectToSpawn);

        if (StartActive)
        {
            for (var i = 0; i < _objectPool.ObjectsPreLoaded; i++)
            {
                SpawnObjectAtRandomPointInRadius();
            }
        }
            


        //InvokeRepeating("SpawnObjectAtRandomPointInRadius", SpawnRate, SpawnRate);
    }

    public void SpawnObjectAtRandomPointInRadius()
    {
        var angle = Random.value * Mathf.PI * 2;
        var x = Mathf.Cos(angle) * SpawnRadius;
        var z = Mathf.Sin(angle) * SpawnRadius;

        //var randomPointOutsideRadius = Random.onUnitSphere * SpawnRadius;
        //randomPointOutsideRadius.y = transform.position.y;
        var pooledObject = _objectPool.GetPooledObject();
        if (!pooledObject)//Ran out of enemies? or enemy pool not working
            return;
        pooledObject.transform.position = new Vector3(x + transform.position.x, transform.position.y, z + transform.position.z);
        pooledObject.SetActive(true);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
    }

}

