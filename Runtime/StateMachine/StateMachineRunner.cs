using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [RequireComponent(typeof(FsmBlackboard))]
    public class StateMachineRunner : MonoBehaviour
    {
        [SerializeField] private FsmState startState;

        private FsmBlackboard blackboard;
        private FsmState currentState;
        
        public FsmBlackboard Blackboard => blackboard;
        public FsmState CurrentState => currentState;

        private void Awake()
        {
            blackboard = GetComponent<FsmBlackboard>();
            ChangeState(startState);
        }

        public void SendEvent(string eventId)
        {
            currentState?.OnEvent(eventId);
        }

        public void ChangeState(FsmState newState)
        {
            if (newState == null)
            {
                Debug.LogError($"[StateMachineRunner] {newState.name} is null... Skipping");
                return;
            }

            currentState?.OnExit();
            currentState = newState;
            currentState?.OnStateEnter(this);
        }

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
