using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MicheliniDev.Utils.EventBus
{
    public static class EventBus
    {
        private static readonly Dictionary<int, UnityEvent> Events = new();

        public static void RegisterReceiver(int eventHash, UnityAction listener)
        {
            if (Events.TryGetValue(eventHash, out UnityEvent thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(eventHash, thisEvent);
            }
        }

        public static void UnsubscribeReceiver(int eventHash, UnityAction listener)
        {
            if (Events.TryGetValue(eventHash, out UnityEvent thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void RaiseEvent(int eventHash)
        {
            if (Events.TryGetValue(eventHash, out UnityEvent thisEvent))
            {
                thisEvent?.Invoke();
            }
        }
    }
}