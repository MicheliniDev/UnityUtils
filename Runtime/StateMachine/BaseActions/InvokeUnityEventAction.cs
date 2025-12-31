using System;
using UnityEngine;
using UnityEngine.Events;

namespace MicheliniDev.Utils.FSM 
{
    [Serializable]
    [Dropdown("Invoke Unity Event")]
    public class InvokeUnityEventAction : FsmAction
    {
        [SerializeField] private UnityEvent unityEvent;

        public override void OnEnter()
        {
            unityEvent.Invoke();
        }
    }
}