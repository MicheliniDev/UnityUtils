using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [RequireComponent(typeof(FsmBlackboard))]
    public class StateMachineRunner : MonoBehaviour
    {
        [SerializeField] private FsmState startState;

        private FsmBlackboard blackboard;
        private FsmState currentState;
        private bool isLocked;

        protected List<FsmState> allStates;

        public FsmBlackboard Blackboard => blackboard;
        public FsmState CurrentState => currentState;

        private void Awake()
        {
            blackboard = GetComponent<FsmBlackboard>();
            InitializeStates();
        }

        private void Start()
        {
            ChangeState(startState);
        }

        protected virtual void InitializeStates()
        {
            allStates = GetComponentsInChildren<FsmState>(true).ToList();
            foreach (var state in allStates)
            {
                state.Initialize(this);
            }
        }

        public void SendEvent(FsmEvent fsmEvent)
        {
            currentState?.OnEvent(fsmEvent);
        }

        public void ChangeState(FsmState newState, StateChangeMode mode = StateChangeMode.Normal)
        {
            if (newState == null)
            {
                Debug.LogError($"[StateMachineRunner] {newState.name} is null... Skipping");
                return;
            }

            if (isLocked && mode == StateChangeMode.Normal)
            {
                Debug.Log($"Ignored transition to {newState.name} because FSM is Locked");
                return;
            }

            currentState?.OnExit();
            currentState = newState;

            isLocked = mode switch
            {
                StateChangeMode.Force => false,
                StateChangeMode.ForceAndLock => true,
                _ => isLocked
            };

            currentState?.OnStateEnter();
        }

        public FsmState GetStateByName(string stateName) => allStates.FirstOrDefault(s => s.name == stateName);

        #region Callbacks
        private void Update()
        {
            currentState?.OnStateUpdate();
        }

        private void FixedUpdate()
        {
            currentState?.OnStateFixedUpdate();
        }

        private void OnCollisionEnter(Collision collision)
        {
            currentState?.OnStateCollisionEnter(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            currentState?.OnStateCollisionEnter2D(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            currentState?.OnStateTriggerEnter(other);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            currentState?.OnStateTriggerEnter2D(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            currentState?.OnStateCollisionStay(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            currentState?.OnStateCollisionStay2D(collision);
        }

        private void OnTriggerStay(Collider other)
        {
            currentState?.OnStateTriggerStay(other);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            currentState?.OnStateTriggerStay2D(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            currentState?.OnStateCollisionExit(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            currentState?.OnStateCollisionExit2D(collision);
        }

        private void OnTriggerExit(Collider other)
        {
            currentState?.OnStateTriggerExit(other);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            currentState?.OnStateTriggerExit2D(collision);
        }
        #endregion
    }
}
