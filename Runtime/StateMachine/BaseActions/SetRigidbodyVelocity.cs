using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Set Rigidbody 3D Velocity")]
    public class SetRigidbodyVelocity : FsmAction
    {
        [SerializeField] private RigidbodyReference rb;
        [SerializeField] private FloatReference velocityX;
        [SerializeField] private FloatReference velocityY;
        [SerializeField] private FloatReference velocityZ;

        public override void OnEnter()
        {
            var rbas = rb.GetValue(Blackboard);
            var x = velocityX.GetValue(Blackboard);

            var velocity = new Vector3(x, velocityY.GetValue(Blackboard), velocityZ.GetValue(Blackboard));
#if UNITY_6000_0_OR_NEWER 
            rbas.linearVelocity = velocity;
#else
            rbas.velocity = velocity;
#endif
        }
    }
}