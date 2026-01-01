using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [System.Serializable]
    [Dropdown("Send Random Event")]
    public class SendRandomEvent : FsmAction
    {
        [SerializeField] private FsmEvent[] events;

        public override void OnEnter()
        {
            var ewent = events[Random.Range(0, events.Length)];
            Owner.SendEvent(ewent);
        }
    }
}