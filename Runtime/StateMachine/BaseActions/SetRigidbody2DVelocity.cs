using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Set Rigidbody 2D Velocity")]
    public class SetRigidbody2DVelocity : FsmAction
    {
        [SerializeField] private Rigidbody2DReference rb;
        [SerializeField] private FloatReference velocityX;
        [SerializeField] private FloatReference velocityY;
        [SerializeField] private bool followXScale;

        public override void OnEnter()
        {
            var rbas = rb.GetValue(Blackboard);
            var x = velocityX.GetValue(Blackboard);

            if (followXScale)
                x *= Mathf.Sign(rbas.transform.localScale.x);

            var velocity = new Vector2(x, velocityY.GetValue(Blackboard));
#if UNITY_6000_0_OR_NEWER 
            rbas.linearVelocity = velocity;
#else
            rbas.velocity = velocity;
#endif
        }
    }
}