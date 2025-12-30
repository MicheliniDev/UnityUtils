using System;
using UnityEngine;

namespace MD.Utils.ServiceLocator
{
    [Serializable]
    public class ServiceEntry
    {
        public string interfaceTypeName;  
        public Component implementation; 
    }
}