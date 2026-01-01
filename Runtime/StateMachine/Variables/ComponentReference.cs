using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [Serializable]
    public class ComponentReference<T> where T : Component
    {
        [SerializeField] private ReferenceMode mode = ReferenceMode.Auto;
        [SerializeField] private GameObjectReference targetObject;
        [SerializeField] private string variableName;

        public T GetValue(FsmBlackboard blackboard)
        {
            switch (mode)
            {
                case ReferenceMode.Auto:
                    return blackboard.Get<T>(true, true);
                case ReferenceMode.Variable:
                    return blackboard.GetVariable<FsmVariable<T>>(variableName).Value;
                case ReferenceMode.Specified:
                    return targetObject.GetValue(blackboard).GetComponent<T>();

                default: return null;
            }
        }
    }

    public enum ReferenceMode
    {
        Auto,
        Specified,
        Variable
    }
}