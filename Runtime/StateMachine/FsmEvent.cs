using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    public class FsmEvent : IEquatable<FsmEvent>
    {
        [SerializeField] private string id;

        public string Id => id;
        public bool IsValid => !string.IsNullOrEmpty(id);

        public FsmEvent(string eventId)
        {
            id = eventId;
        }

        public bool Equals(FsmEvent other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(id, other.id);
        }

        public static bool operator ==(FsmEvent eventA, FsmEvent eventB)
        {
            if (ReferenceEquals(eventA, null))
            {
                return ReferenceEquals(eventB, null);
            }
            return eventA.Equals(eventB);
        }

        public static bool operator !=(FsmEvent eventA, FsmEvent eventB) => !(eventA == eventB);

        public override bool Equals(object obj) => obj is FsmEvent other && Equals(other);

        public static implicit operator string(FsmEvent fsmEvent) => fsmEvent?.id;
        public static implicit operator FsmEvent(string eventId) => new FsmEvent(eventId);

        public static implicit operator bool(FsmEvent fsmEvent) => fsmEvent != null;

        public override string ToString() => id;
        public override int GetHashCode() => id?.GetHashCode() ?? 0;
    }
}