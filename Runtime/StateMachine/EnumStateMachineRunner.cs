using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    /*
        Made in case you need a more precise control over the machine,
        Like for a Player FSM, for example
    */
    public class EnumStateMachineRunner<T> : StateMachineRunner where T : Enum
    {
        [SerializeField] private SerializableDictionary<T, FsmState> states;

        public void ChangeState(T stateType) 
        { 
            if (StateExists(stateType, out var state))
            {
                ChangeState(state);
            }
        }

        public bool IsInState(T stateType)
        {
            return StateExists(stateType, out var statate) && CurrentState == statate;
        }

        private bool StateExists(T stateType, out FsmState state)
        {
            if (!states.TryGetValue(stateType, out state))
            {
                Debug.LogWarning($"[{name}] No state mapped for enum key: {stateType}");
                return false;
            }
            return true;
        }
    }
}
