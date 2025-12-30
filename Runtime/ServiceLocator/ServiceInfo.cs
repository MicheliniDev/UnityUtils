using UnityEngine;
using System;
using System.Collections.Generic;

namespace MD.Utils.ServiceLocator
{
    public class ServiceInfo
    {
        private readonly Dictionary<Type, object> services = new();

        public ServiceInfo Register<T>(T service)
        {
            return Register(typeof(T), service);
        }

        public ServiceInfo Register(Type type, object service)
        {
            if (!services.TryAdd(type, service))
            {
                Debug.LogWarning($"Service {type.Name} already registered.");
            }
            return this;
        }

        public T Get<T>()
        {
            var type = typeof(T);

            if (!services.TryGetValue(type, out var service))
                throw new Exception($"Service {type.Name} not found. Did you forget to Register it?");

            return (T)service;
        }
    }
}