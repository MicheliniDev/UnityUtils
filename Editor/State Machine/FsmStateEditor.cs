using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [CustomEditor(typeof(FsmState))]
    public class FsmStateEditor : UnityEditor.Editor
    {
        private SerializedProperty actionsProp;

        private void OnEnable()
        {
            actionsProp = serializedObject.FindProperty("actions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Space(2f);

            DrawDefaultInspectorExcept("actions");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

            FsmEditorUtils.DrawSerializeReferenceList(actionsProp, typeof(FsmAction));

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDefaultInspectorExcept(string skipPropName)
        {
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (prop.name != "m_Script" && prop.name != skipPropName)
                    {
                        EditorGUILayout.PropertyField(prop, true);
                    }
                }
                while (prop.NextVisible(false));
            }
        }
    }
}
