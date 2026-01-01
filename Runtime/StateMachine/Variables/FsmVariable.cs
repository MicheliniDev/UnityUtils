using System;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    public abstract class FsmVariableBase
    {
        public string Name;
    }

    [Serializable]
    public abstract class FsmVariable<T> : FsmVariableBase
    {
        public T Value;
    }
}