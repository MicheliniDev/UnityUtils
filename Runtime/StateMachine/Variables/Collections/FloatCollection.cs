using MicheliniDev.Utils.FSM;
using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Basic/Float")]
    public class FloatVariable : FsmVariable<float> { }


    [Serializable]
    public class FloatReference : FsmReference<float, FloatVariable> { }
}