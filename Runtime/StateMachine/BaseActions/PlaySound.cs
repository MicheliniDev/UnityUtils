using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Play Sound")]
    public class PlaySound : FsmAction
    {
        [SerializeField] private AudioSourceReference audioSource;
        [SerializeField] private AudioClipReference clip;
        [SerializeField] private bool playOneShot;

        public override void OnEnter()
        {
            var source = audioSource.GetValue(Blackboard);
            if (playOneShot)
                source.PlayOneShot(clip.GetValue(Blackboard));
            else
            {
                source.clip = clip.GetValue(Blackboard);
                source.Play();
            }
        }
    }
}