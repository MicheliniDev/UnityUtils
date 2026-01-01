using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MicheliniDev.Utils.ServiceLocator
{
    public static class ServiceLocator
    {
        private static ServiceInfo global;
        private static Dictionary<Scene, ServiceInfo> sceneContainers;

        public static ServiceInfo Global => global ??= new ServiceInfo();

        public static ServiceInfo ForScene(Scene scene)
        {
            if (sceneContainers == null)
            {
                sceneContainers = new Dictionary<Scene, ServiceInfo>();
                SceneManager.sceneUnloaded += OnSceneUnloaded;
            }

            if (!sceneContainers.ContainsKey(scene))
            {
                sceneContainers[scene] = new ServiceInfo();
            }

            return sceneContainers[scene];
        }
    
        private static void OnSceneUnloaded(Scene scene)
        {
            if (sceneContainers != null && sceneContainers.ContainsKey(scene))
            {
                Debug.Log($"Cleaning up services for scene: {scene.name}");
                sceneContainers.Remove(scene);
            }
        }
    }
}