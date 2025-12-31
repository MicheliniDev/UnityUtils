using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [System.Serializable]
    public abstract class FsmAction
    {
        protected StateMachineRunner Owner { get; private set; }
        protected FsmBlackboard Blackboard { get; private set; }

        public virtual void Initialize(StateMachineRunner owner)
        {
            Owner = owner;
            Blackboard = owner.Blackboard;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnExit() { }
        public virtual void OnCollisionEnter(Collision collision) { }
        public virtual void OnCollisionEnter2D(Collision2D collision) { }
        public virtual void OnTriggerEnter(Collider other) { }
        public virtual void OnTriggerEnter2D(Collider2D collision) { }
        public virtual void OnCollisionStay(Collision collision) { }
        public virtual void OnCollisionStay2D(Collision2D collision) { }
        public virtual void OnTriggerStay(Collider other) { }
        public virtual void OnTriggerStay2D(Collider2D collision) { }
        public virtual void OnCollisionExit(Collision collision) { }
        public virtual void OnCollisionExit2D(Collision2D collision) { }
        public virtual void OnTriggerExit(Collider other) { }
        public virtual void OnTriggerExit2D(Collider2D collision) { }
    }
}
