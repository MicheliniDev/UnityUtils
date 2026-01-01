using MicheliniDev.Utils.FSM;
using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Basic/Bool")]
    public class BoolVariable : FsmVariable<bool> { }

    [Serializable]
    public class BoolReference : FsmReference<bool, BoolVariable> { }
}