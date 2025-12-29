using System;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    public interface IFsmReference<T>
    {
        bool UseBlackboard { get; set; }
        string VariableName { get; set; }
        T ConstantValue { get; set; }
    }

    [Serializable]
    public class FloatReference : IFsmReference<float>
    {
        [SerializeField] private bool useBlackboard;
        [SerializeField] private string variableName;
        [SerializeField] private float constantValue;

        public bool UseBlackboard { get => useBlackboard; set => useBlackboard = value; }
        public string VariableName { get => variableName; set => variableName = value; }
        public float ConstantValue { get => constantValue; set => constantValue = value; }

        public float GetValue(FsmBlackboard blackboard)
        {
            if (useBlackboard && !string.IsNullOrEmpty(variableName))
            {
                var variable = blackboard.GetVariable<FloatVariable>(variableName);
                return variable != null ? variable.Value : constantValue;
            }
            return constantValue;
        }
    }

    [Serializable]
    public class IntReference : IFsmReference<int>
    {
        [SerializeField] private bool useBlackboard;
        [SerializeField] private string variableName;
        [SerializeField] private int constantValue;

        public bool UseBlackboard { get => useBlackboard; set => useBlackboard = value; }
        public string VariableName { get => variableName; set => variableName = value; }
        public int ConstantValue { get => constantValue; set => constantValue = value; }

        public int GetValue(FsmBlackboard blackboard)
        {
            if (useBlackboard && !string.IsNullOrEmpty(variableName))
            {
                var variable = blackboard.GetVariable<IntVariable>(variableName);
                return variable != null ? variable.Value : constantValue;
            }
            return constantValue;
        }
    }

    [Serializable]
    public class BoolReference : IFsmReference<bool>
    {
        [SerializeField] private bool useBlackboard;
        [SerializeField] private string variableName;
        [SerializeField] private bool constantValue;

        public bool UseBlackboard { get => useBlackboard; set => useBlackboard = value; }
        public string VariableName { get => variableName; set => variableName = value; }
        public bool ConstantValue { get => constantValue; set => constantValue = value; }

        public bool GetValue(FsmBlackboard blackboard)
        {
            if (useBlackboard && !string.IsNullOrEmpty(variableName))
            {
                var variable = blackboard.GetVariable<BoolVariable>(variableName);
                return variable != null ? variable.Value : constantValue;
            }
            return constantValue;
        }
    }
}