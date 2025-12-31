using System;
using System.Linq;
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

        protected override void InitializeStates()
        {
            base.allStates = states.Values.ToList();
            foreach (var state in states.Values)
            {
                state?.Initialize(this);
            }
        }
        
        public FsmState GetStateByEnum(T stateType) => states[stateType];

        public void ChangeState(T stateType, StateChangeMode mode = StateChangeMode.Normal) 
        { 
            if (StateExists(stateType, out var state))
            {
                ChangeState(state, mode);
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
