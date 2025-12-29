using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    public static class ReferenceDrawerUtils
    {
        public static void DrawReference(Rect position, SerializedProperty property, System.Type variableType)
        {
            SerializedProperty useBlackboard = property.FindPropertyRelative("useBlackboard");
            SerializedProperty constantValue = property.FindPropertyRelative("constantValue");
            SerializedProperty variableName = property.FindPropertyRelative("variableName");

            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect modeRect = new Rect(position.x, position.y, position.width, singleLineHeight);
            Rect contentRect = new Rect(position.x, position.y + singleLineHeight + spacing, position.width, singleLineHeight);

            modeRect.x -= 14f;
            modeRect.width += 14f;

            contentRect.x -= 13.5f;
            contentRect.width += 13.5f;

            int currentMode = useBlackboard.boolValue ? 1 : 0;
            string[] modes = { "Regular", "Blackboard Variable" };

            int newMode = EditorGUI.Popup(modeRect, currentMode, modes);
            useBlackboard.boolValue = (newMode == 1);

            if (!useBlackboard.boolValue)
                EditorGUI.PropertyField(contentRect, constantValue, GUIContent.none);
            else
                DrawBlackboardSelector(contentRect, variableName, variableType, property);
        }

        private static void DrawBlackboardSelector(Rect rect, SerializedProperty nameProp, System.Type varType, SerializedProperty rootProp)
        {
            FsmBlackboard blackboard = null;

            var target = rootProp.serializedObject.targetObject as Component;
            if (target != null)
            {
                blackboard = target.GetComponentInParent<FsmBlackboard>();
                if (blackboard == null) blackboard = target.GetComponentInChildren<FsmBlackboard>(true);
            }

            if (blackboard == null || blackboard.Variables == null)
            {
                EditorGUI.PropertyField(rect, nameProp, GUIContent.none);
                return;
            }

            var validVariables = blackboard.Variables
                .Where(v => v != null && v.GetType() == varType)
                .Select(v => v.Name)
                .ToList();

            if (validVariables.Count == 0)
            {
                EditorGUI.LabelField(rect, "<No Variables of type>", EditorStyles.miniLabel);
                return;
            }

            int index = validVariables.IndexOf(nameProp.stringValue);
            int newIndex = EditorGUI.Popup(rect, index, validVariables.ToArray());

            if (newIndex >= 0 && newIndex < validVariables.Count)
            {
                nameProp.stringValue = validVariables[newIndex];
            }
            else if (string.IsNullOrEmpty(nameProp.stringValue) && validVariables.Count > 0)
            {
                nameProp.stringValue = validVariables[0];
            }
        }
    }
}
