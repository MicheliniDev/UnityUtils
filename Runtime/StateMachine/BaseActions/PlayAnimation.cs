using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Play Animation")]
    public class PlayAnimation : FsmAction
    {
        [SerializeField] private FsmEvent onAnimationEnd;

        [SerializeField] private AnimatorReference animator;
        [SerializeField] private StringReference animationName;
        [SerializeField] private IntReference layerIndex;
        [SerializeField] private FloatReference normalizedTime;

        private Coroutine playAnimRoutine;

        public override void OnEnter()
        {
            playAnimRoutine = Owner.StartCoroutine(PlayAnimationWithEvent(animator.GetValue(Blackboard)));
        }

        public override void OnExit()
        {
            Owner.StopCoroutine(playAnimRoutine);
        }

        private IEnumerator PlayAnimationWithEvent(Animator anim) 
        {
            anim.Play(animationName.GetValue(Blackboard), layerIndex.GetValue(Blackboard), normalizedTime.GetValue(Blackboard));
            
            yield return null;

            while (anim.GetCurrentAnimatorStateInfo(layerIndex.GetValue(Blackboard)).normalizedTime <= .99f)
            {
                yield return null;
            }

            Owner.SendEvent(onAnimationEnd);
        }
    }
}