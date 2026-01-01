using MicheliniDev.Utils.FSM;
using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    [Dropdown("Unity/GameObject")]
    public class GameObjectVariable : FsmVariable<GameObject> { }
    [Serializable] public class GameObjectReference : FsmReference<GameObject, GameObjectVariable> { }
}