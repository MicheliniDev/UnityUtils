using System;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Send Event")]
    public class SendFsmEvent : FsmAction
    {
        [SerializeField] private FsmEvent eventt;

        public override void OnEnter()
        {
            Owner.SendEvent(eventt);
        }
    }
}