using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/Rigidbody")]
    public class RigidbodyVariable : FsmVariable<Rigidbody> { }
 
    [Serializable]
    public class RigidbodyReference : ComponentReference<Rigidbody> { }
}