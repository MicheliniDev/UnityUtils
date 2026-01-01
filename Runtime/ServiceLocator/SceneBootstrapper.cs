using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicheliniDev.Utils.ServiceLocator
{
    [DefaultExecutionOrder(-20)]
    public class SceneBootstrapper : MonoBehaviour
    {
        [SerializeField] private bool isGlobalSource;
        [SerializeField] private List<ServiceEntry> services;

        private void Awake()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var serviceToRegister = isGlobalSource ?
                ServiceLocator.Global :
                ServiceLocator.ForScene(gameObject.scene);
            
            foreach (var service in services)
            {
                if (service.implementation == null) continue;

                var type = Type.GetType(service.interfaceTypeName);
                serviceToRegister.Register(type, service.implementation);
            }
        }
    }
}