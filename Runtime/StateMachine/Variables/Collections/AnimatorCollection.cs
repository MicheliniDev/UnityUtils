using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/Animator")]
    public class AnimatorVariable : FsmVariable<Animator> { }
 
    [Serializable] public class AnimatorReference : ComponentReference<Animator> { }
}