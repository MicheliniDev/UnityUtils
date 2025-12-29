using MicheliniDev.Utils.ScriptableVariables;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.Editor
{
    [CustomEditor(typeof(ScriptableVariable<>), true)] 
    public class ScriptableVariableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawPropertiesExcluding(serializedObject, "m_Script", "_value");
            
            serializedObject.ApplyModifiedProperties();

            dynamic targetVariable = target;

            EditorGUI.BeginChangeCheck();

            object currentValue = targetVariable.Value;
            object newValue = currentValue;

            if (currentValue is int iVal)
            {
                newValue = EditorGUILayout.IntField("Current Value", iVal);
            }
            else if (currentValue is float fVal)
            {
                newValue = EditorGUILayout.FloatField("Current Value", fVal);
            }
            else if (currentValue is bool bVal)
            {
                newValue = EditorGUILayout.Toggle("Current Value", bVal);
            }
            else if (currentValue is string sVal)
            {
                newValue = EditorGUILayout.TextField("Current Value", sVal);
            }
            else 
            {
                EditorGUILayout.LabelField("Current Value", currentValue?.ToString() ?? "null");
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change Variable Value");
                
                targetVariable.Value = newValue;
                
                EditorUtility.SetDirty(target);
            }
        }
    }
}