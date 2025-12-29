using System.Collections;
using UnityEngine;

namespace MicheliniDev.Utils.ObjectPool
{
    public class PoolObject : MonoBehaviour
    {
        private enum PoolReturnReason
        {
            Explicit, //Called by external script or animation event
            Timer
        }
    
        [SerializeField] private PoolReturnReason reason;
        [SerializeField] private float ReturnTimer;

        [HideInInspector] public ObjectPool Pool;

        private void OnEnable()
        {
            if (reason == PoolReturnReason.Timer)
                StartCoroutine(ReturnToPoolTimerRoutine());
        }

        private IEnumerator ReturnToPoolTimerRoutine()
        {
            yield return new WaitForSeconds(ReturnTimer);
            ReturnToPool();
        }

        public void ReturnToPool()
        {
            Pool.Return(gameObject);
        }
    }
}