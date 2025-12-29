using UnityEngine;
using UnityEngine.Events;

namespace MicheliniDev.Utils.EventBus
{
    public class EventListener : MonoBehaviour
    {
        [EventBus] public string EventName;
        public UnityEvent Response;

        private int eventHash;

        public void Awake()
        {
            eventHash = Animator.StringToHash(EventName);
            EventBus.RegisterReceiver(eventHash, Response.Invoke);
        }

        public void OnDestroy()
        {
            EventBus.UnsubscribeReceiver(eventHash, Response.Invoke);
        }
    }
}