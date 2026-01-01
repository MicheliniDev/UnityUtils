using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Basic/Int")]
    public class IntVariable : FsmVariable<int> { }

    [Serializable]
    public class IntReference : FsmReference<int, IntVariable> { }
}