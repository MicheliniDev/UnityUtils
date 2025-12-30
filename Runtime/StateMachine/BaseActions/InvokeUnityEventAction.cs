using System;
using UnityEngine;
using UnityEngine.Events;

namespace MicheliniDev.Utils.FSM 
{
    [Serializable]
    public class InvokeUnityEventAction : FsmAction
    {
        [SerializeField] private UnityEvent unityEvent;

        public override void OnEnter(StateMachineRunner owner)
        {
            base.OnEnter(owner);
            unityEvent.Invoke();
        }
    }
}