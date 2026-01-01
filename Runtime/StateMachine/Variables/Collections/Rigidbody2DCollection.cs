using MicheliniDev.Utils.FSM;
using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/Rigidbody2D")]
    public class Rigidbody2DVariable : FsmVariable<Rigidbody2D> { }
    [Serializable] public class Rigidbody2DReference : ComponentReference<Rigidbody2D> { }
}