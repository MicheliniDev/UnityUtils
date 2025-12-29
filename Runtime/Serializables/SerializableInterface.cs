using UnityEngine;
using System;

namespace MicheliniDev.Utils
{
    [Serializable]
    public class SerializableInterface<T> where T : class
    {
        [SerializeField] private UnityEngine.Object targetObject;

        private T component;

        public T Value
        {
            get
            {
                if (targetObject == null) 
                    return null;

                return targetObject switch
                {
                    T interfaceType => interfaceType,
                    GameObject go => GetInterfaceComponent(go),
                    _ => null
                };
            }
        }

        private T GetInterfaceComponent(GameObject go)
        {
            if (component == null)
            {
                component = go.GetComponent<T>();
            }
            return component;
        }

        public void Validate()
        {
            if (targetObject == null || Value != null) 
                return;
            
            Debug.LogWarning($"{targetObject.name} does not implement {typeof(T).Name}!");
            targetObject = null;
        }
    }
}