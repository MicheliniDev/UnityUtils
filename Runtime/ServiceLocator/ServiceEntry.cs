using System;
using UnityEngine;

namespace MicheliniDev.Utils.ServiceLocator
{
    [Serializable]
    public class ServiceEntry
    {
        public string interfaceTypeName;  
        public Component implementation; 
    }
}