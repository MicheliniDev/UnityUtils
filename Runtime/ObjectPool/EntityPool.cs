using System.Collections.Generic;
using UnityEngine;

namespace MicheliniDev.Utils.ObjectPool
{
    public class EntityPool : MonoBehaviour
    {
        [SerializeField] private List<ObjectPool> startupPools;

        private void Awake()
        {
            foreach (var pool in startupPools)
                PoolManager.Instance.SetStartupPools(startupPools);
        }

        private void OnDestroy()
        {
            foreach (var pool in startupPools)
                PoolManager.Instance.DestroyPool(pool);
        }
    }

}