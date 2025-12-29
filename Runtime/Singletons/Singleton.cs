using UnityEngine;

namespace MicheliniDev.Utils
{
    [DefaultExecutionOrder(-20)]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static bool isQuitting;

        public static T Instance
        {
            get
            {
                if (isQuitting)
                    return null;

                if (instance)
                    return instance;

                instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);

                if (!instance)
                {
                    instance = new GameObject(typeof(T).FullName + "(Auto-Generated)", typeof(T)).GetComponent<T>();
                }

                if (!instance.transform.parent)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            isQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                isQuitting = true;
        }
    }
}