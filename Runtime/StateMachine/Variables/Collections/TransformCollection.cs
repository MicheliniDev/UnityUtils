using MicheliniDev.Utils.FSM;
using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/Transform")]
    public class TransformVariable : FsmVariable<Transform> { }

    [Serializable] public class TransformReference : FsmReference<Transform, TransformVariable> { }
}