using MicheliniDev.Utils.FSM;
using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Basic/String")]
    public class StringVariable : FsmVariable<string> { }

    [Serializable]
    public class StringReference : FsmReference<string, StringVariable> { }
}