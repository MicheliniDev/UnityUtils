using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    public class FsmTransition
    {
        [Tooltip("The Event ID that triggers this transition.")]
        public FsmEvent Event;

        [Tooltip("The state to switch to when the event is triggered.")]
        public FsmState TargetState;
    }
}