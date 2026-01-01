using MicheliniDev.Utils.FSM;
using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/Vector2")]
    public class Vector2Variable : FsmVariable<Vector2> { }

    [Serializable]
    public class Vector2Reference : FsmReference<Vector2, Vector2Variable> { }
}