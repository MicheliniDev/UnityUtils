using System.Collections.Generic;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    public class FsmState : MonoBehaviour
    {
        [SerializeReference] private List<FsmAction> actions;
        [SerializeField] private List<FsmTransition> transitions;

        private StateMachineRunner owner;

        public void Initialize(StateMachineRunner controller)
        {
            owner = controller;
            foreach (var action in actions)
                action?.Initialize(controller);
        }

        public void OnEvent(FsmEvent fsmEvent)
        {
            foreach (var transition in transitions)
            {
                if (!transition.Event)
                {
                    Debug.Log("Transition is null");
                    continue;
                }
                
                if (transition.Event == fsmEvent)
                {
                    owner.ChangeState(transition.TargetState);
                    return;
                }
            }
        }

        public void OnStateEnter()
        {
            foreach (var action in actions) 
                action?.OnEnter();
        }

        public void OnStateUpdate()
        {
            foreach (var action in actions) 
                action?.OnUpdate();
        }

        public void OnStateFixedUpdate()
        {
            foreach (var action in actions)
                action?.OnFixedUpdate();
        }

        public void OnExit()
        {
            foreach (var action in actions)
                action?.OnExit();
        }

        public void OnStateCollisionEnter(Collision collision)
        {
            foreach (var action in actions)
                action?.OnCollisionEnter(collision);
        }

        public void OnStateCollisionEnter2D(Collision2D collision)
        {
            foreach (var action in actions)
                action?.OnCollisionEnter2D(collision);
        }

        public void OnStateTriggerEnter(Collider other)
        {
            foreach (var action in actions)
                action?.OnTriggerEnter(other);
        }

        public void OnStateTriggerEnter2D(Collider2D collision)
        {
            foreach (var action in actions)
                action?.OnTriggerEnter2D(collision);
        }

        public void OnStateCollisionStay(Collision collision)
        {
            foreach (var action in actions)
                action?.OnCollisionStay(collision);
        }

        public void OnStateCollisionStay2D(Collision2D collision)
        {
            foreach (var action in actions)
                action?.OnCollisionStay2D(collision);
        }

        public void OnStateTriggerStay(Collider other)
        {
            foreach (var action in actions)
                action?.OnTriggerStay(other);
        }

        public void OnStateTriggerStay2D(Collider2D collision)
        {
            foreach (var action in actions)
                action?.OnTriggerStay2D(collision);
        }

        public void OnStateCollisionExit(Collision collision)
        {
            foreach (var action in actions)
                action?.OnCollisionExit(collision);
        }

        public void OnStateCollisionExit2D(Collision2D collision)
        {
            foreach (var action in actions)
                action?.OnCollisionExit2D(collision);
        }

        public void OnStateTriggerExit(Collider other)
        {
            foreach (var action in actions)
                action?.OnTriggerExit(other);
        }

        public void OnStateTriggerExit2D(Collider2D collision)
        {
            foreach (var action in actions)
                action?.OnTriggerExit2D(collision);
        }
    }
}
