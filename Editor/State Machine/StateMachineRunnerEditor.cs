using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [CustomEditor(typeof(StateMachineRunner))]
    public class StateMachineRunnerEditor : UnityEditor.Editor
    {
        private SerializedProperty startStateProp;

        private void OnEnable()
        {
            startStateProp = serializedObject.FindProperty("startState");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            StateMachineRunner owner = (StateMachineRunner)target;

            GUI.enabled = Application.isPlaying;
            EditorGUILayout.ObjectField("Current State", owner.CurrentState, typeof(FsmState), true);
            GUI.enabled = true;

            EditorGUILayout.PropertyField(startStateProp);

            DrawPropertiesExcluding(serializedObject, "m_Script", "startState", "currentState");

            serializedObject.ApplyModifiedProperties();
        }
    }
}