using UnityEngine;

namespace MicheliniDev.Utils.EventBus
{
    public class EventSender : MonoBehaviour
    {
        [EventBus] public string EventName;

        private int eventHash;

        public void RaiseEvent()
        {
            eventHash = Animator.StringToHash(EventName);
            EventBus.RaiseEvent(eventHash);
        }
    }
}