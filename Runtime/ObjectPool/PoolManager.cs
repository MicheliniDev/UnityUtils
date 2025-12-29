using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicheliniDev.Utils.ObjectPool
{
    public class PoolManager : Singleton<PoolManager>
    {
        private const int DEFAULT_POOL_SIZE = 10;

        [SerializeField] 
        private List<ObjectPool> startupPools = new();
        private Dictionary<GameObject, ObjectPool> allPools = new();
    
        public Transform PoolHolder;
    
        private void Awake()
        {
            SetStartupPools(startupPools);
        }

        public void SetStartupPools(List<ObjectPool> pools)
        {
            foreach (var pool in pools)
            {
                allPools.Add(pool.prefab, pool);
                pool.Initialize();
            }
        }

        public GameObject SpawnObject(GameObject prefab)
        {
            return SpawnObject(prefab, Vector3.zero, Quaternion.identity);
        }
    
        public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, Action<GameObject> OnGet = null)
        {
            GameObject poolObject = null;
        
            if (allPools.TryGetValue(prefab, out var pool))
            {
                poolObject = pool.Get();
            }
            else
            {
                var newPool = new ObjectPool(prefab, DEFAULT_POOL_SIZE);
                allPools.Add(newPool.prefab, newPool);
                poolObject = newPool.Get();
            }
            poolObject.SetActive(true);
            poolObject.transform.position = position;
            poolObject.transform.rotation = rotation;

            poolObject.transform.parent = !parent ? PoolHolder : parent;
            OnGet?.Invoke(poolObject);
            return poolObject;
        }
    
        public void DestroyPool(ObjectPool pool)
        {
            allPools[pool.prefab].Destroy();
            allPools.Remove(pool.prefab);
        }
    }
}