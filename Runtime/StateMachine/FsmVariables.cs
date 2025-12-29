using UnityEngine;
using System;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    public abstract class FsmVariable
    {
        public string Name;
    }

    [Serializable]
    [Dropdown("Basic/Float")]
    public class FloatVariable : FsmVariable
    {
        public float Value;
    }

    [Serializable]
    [Dropdown("Basic/Int")]
    public class IntVariable : FsmVariable
    {
        public int Value;
    }

    [Serializable]
    [Dropdown("Basic/Bool")]
    public class BoolVariable : FsmVariable
    {
        public bool Value;
    }

    [Serializable]
    [Dropdown("Unity/Vector3")]
    public class Vector3Variable : FsmVariable
    {
        public Vector3 Value;
    }
}