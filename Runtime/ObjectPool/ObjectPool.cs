using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MicheliniDev.Utils.ObjectPool
{
    [Serializable]
    public class ObjectPool
    {
        public GameObject prefab;
        public int poolSize;
        private Queue<GameObject> inactiveObjects;
        private HashSet<GameObject> activeObjects;
    
        public void Initialize()
        {
            inactiveObjects = new();
            activeObjects = new();
        
            for (int i = 0; i <= poolSize; i++)
            {
                var instance = Object.Instantiate(prefab, PoolManager.Instance.PoolHolder);
                instance.GetComponent<PoolObject>().Pool = this;
                instance.SetActive(false);
                inactiveObjects.Enqueue(instance);
            }
        }

        public void Destroy()
        {
            foreach (var gameObject in inactiveObjects)
                Object.DestroyImmediate(gameObject);
            foreach (var gameObject in activeObjects)
                Object.DestroyImmediate(gameObject);

            inactiveObjects = null;
            activeObjects = null;
        }
    
        public GameObject Get()
        {
            GameObject gameObject = null;

            if (inactiveObjects.Count != 0)
                gameObject = inactiveObjects.Dequeue();
            else if (PoolManager.Instance.PoolHolder)
                gameObject = Object.Instantiate(prefab, PoolManager.Instance.PoolHolder);

            activeObjects.Add(gameObject);
            return gameObject;
        }

        public void Return(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.parent = PoolManager.Instance.PoolHolder;
            activeObjects.Remove(gameObject);
            inactiveObjects.Enqueue(gameObject);
        }
    
        public ObjectPool(GameObject prefab, int poolSize)
        {
            this.prefab = prefab;
            this.poolSize = poolSize;
            Initialize();
        }
    }
}