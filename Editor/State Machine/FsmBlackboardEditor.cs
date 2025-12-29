using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [CustomEditor(typeof(FsmBlackboard))]
    public class FsmBlackboardEditor : UnityEditor.Editor
    {
        private SerializedProperty variablesProp;
        private SerializedProperty eventIdsProp;

        private void OnEnable()
        {
            variablesProp = serializedObject.FindProperty("Variables");
            eventIdsProp = serializedObject.FindProperty("Events");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Space(4f);

            EditorGUILayout.LabelField("FSM Events", EditorStyles.boldLabel);
            DrawStringList(eventIdsProp);

            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Blackboard Data", EditorStyles.boldLabel);
            FsmEditorUtils.DrawSerializeReferenceList(variablesProp, typeof(FsmVariable));

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawStringList(SerializedProperty listProperty)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(element, GUIContent.none);

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    listProperty.DeleteArrayElementAtIndex(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(5f);

            if (GUILayout.Button("Add Event"))
            {
                listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
                listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).stringValue = string.Empty;
            }

            EditorGUILayout.EndVertical();
        }
    }
}
