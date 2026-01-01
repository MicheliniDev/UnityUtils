using MicheliniDev.Utils.FSM;
using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/Vector3")]
    public class Vector3Variable : FsmVariable<Vector3> { }

    [Serializable]
    public class Vector3Reference : FsmReference<Vector3, Vector3Variable> { }
}