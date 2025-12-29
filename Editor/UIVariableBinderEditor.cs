using MicheliniDev.Utils.ScriptableVariables;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(UIVariableBinder))]
    [CanEditMultipleObjects]
    public class UIVariableBinderEditor : UnityEditor.Editor
    {
        SerializedProperty bindType;
        SerializedProperty floatSource, intSource, stringSource, boolSource;
        SerializedProperty targetSlider, targetImage, targetText, targetToggle, targetGameObject;
        SerializedProperty formatString;

        void OnEnable()
        {
            bindType = serializedObject.FindProperty("bindType");
            
            floatSource = serializedObject.FindProperty("floatSource");
            intSource = serializedObject.FindProperty("intSource");
            stringSource = serializedObject.FindProperty("stringSource");
            boolSource = serializedObject.FindProperty("boolSource");

            targetSlider = serializedObject.FindProperty("targetSlider");
            targetImage = serializedObject.FindProperty("targetImage");
            targetText = serializedObject.FindProperty("targetText");
            targetToggle = serializedObject.FindProperty("targetToggle");
            targetGameObject = serializedObject.FindProperty("targetGameObject");
            
            formatString = serializedObject.FindProperty("formatString");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(bindType);
            EditorGUILayout.Space(5);

            VariableType type = (VariableType)bindType.enumValueIndex;

            switch (type)
            {
                case VariableType.Float:
                    EditorGUILayout.PropertyField(floatSource, new GUIContent("Float Variable"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(targetSlider, new GUIContent("Slider (Value)"));
                    EditorGUILayout.PropertyField(targetImage, new GUIContent("Image (Fill)"));
                    EditorGUILayout.PropertyField(targetText, new GUIContent("Text (Value)"));
                    if (targetText.objectReferenceValue != null)
                        EditorGUILayout.PropertyField(formatString);
                    break;

                case VariableType.Int:
                    EditorGUILayout.PropertyField(intSource, new GUIContent("Int Variable"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(targetText, new GUIContent("Text (Value)"));
                    if (targetText.objectReferenceValue != null)
                        EditorGUILayout.PropertyField(formatString);
                    break;

                case VariableType.String:
                    EditorGUILayout.PropertyField(stringSource, new GUIContent("String Variable"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(targetText, new GUIContent("Text (Value)"));
                    if (targetText.objectReferenceValue != null)
                        EditorGUILayout.PropertyField(formatString);
                    break;

                case VariableType.Bool:
                    EditorGUILayout.PropertyField(boolSource, new GUIContent("Bool Variable"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(targetToggle, new GUIContent("Toggle (Is On)"));
                    EditorGUILayout.PropertyField(targetGameObject, new GUIContent("GameObject (Active)"));
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}