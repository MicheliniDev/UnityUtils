using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [CustomEditor(typeof(FsmBlackboard))]
    public class FsmBlackboardEditor : UnityEditor.Editor
    {
        private SerializedProperty variablesProp;
        private SerializedProperty eventsProp;

        private void OnEnable()
        {
            variablesProp = serializedObject.FindProperty("Variables");
            eventsProp = serializedObject.FindProperty("Events");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Space(4f);

            EditorGUILayout.LabelField("FSM Events", EditorStyles.boldLabel);
            DrawEventDefinitionList(eventsProp);

            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Blackboard Data", EditorStyles.boldLabel);
            FsmEditorUtils.DrawSerializeReferenceList(variablesProp, typeof(FsmVariableBase));

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawEventDefinitionList(SerializedProperty listProperty)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
                SerializedProperty idProp = element.FindPropertyRelative("id");

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(idProp, GUIContent.none);

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
                var newElem = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
                newElem.FindPropertyRelative("id").stringValue = "New Event";
            }

            EditorGUILayout.EndVertical();
        }
    }
}