using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    public interface IFsmReference<T>
    {
        T GetValue(FsmBlackboard blackboard);
    }

    [Serializable]
    public abstract class FsmReference<TBaseVariable, TFsmVariable> : IFsmReference<TBaseVariable>
        where TFsmVariable : FsmVariable<TBaseVariable>
    {
        [SerializeField] protected bool useBlackboard;
        [SerializeField] protected string variableName;
        [SerializeField] protected TBaseVariable constantValue;

        public TBaseVariable GetValue(FsmBlackboard blackboard)
        {
            if (useBlackboard && !string.IsNullOrEmpty(variableName))
            {
                var variable = blackboard.GetVariable<TFsmVariable>(variableName);
                return variable != null ? variable.Value : constantValue;
            }
            return constantValue;
        }
    }
}