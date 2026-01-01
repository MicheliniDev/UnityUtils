using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [CustomPropertyDrawer(typeof(ComponentReference<>), true)]
    public class ComponentReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var mode = property.FindPropertyRelative("mode");
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (mode != null)
            {
                ReferenceMode currentMode = (ReferenceMode)mode.enumValueIndex;

                if (currentMode == ReferenceMode.Specified)
                {
                    var targetObject = property.FindPropertyRelative("targetObject");
                    height += EditorGUI.GetPropertyHeight(targetObject);

                    if (!IsValid(property))
                    {
                        height += EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
                else if (currentMode == ReferenceMode.Variable)
                {
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            height += 3f;
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var mode = property.FindPropertyRelative("mode");
            var targetObject = property.FindPropertyRelative("targetObject");
            var variableName = property.FindPropertyRelative("variableName");

            Type componentType = GetComponentType();

            Type variableType = null;
            if (componentType != null)
            {
                var genericBase = typeof(FsmVariable<>).MakeGenericType(componentType);
                variableType = TypeCache.GetTypesDerivedFrom(genericBase).FirstOrDefault();
            }

            bool hasVariableCounterpart = variableType != null;

            Rect modeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            ReferenceMode currentMode = (ReferenceMode)mode.enumValueIndex;

            if (!hasVariableCounterpart && currentMode == ReferenceMode.Variable)
            {
                currentMode = ReferenceMode.Auto;
                mode.enumValueIndex = (int)ReferenceMode.Auto;
            }

            EditorGUI.BeginChangeCheck();

            Enum displayedMode;
            if (hasVariableCounterpart)
            {
                displayedMode = (ReferenceMode)EditorGUI.EnumPopup(modeRect, label, currentMode);
            }
            else
            {
                string[] options = { "Auto", "Specified" };
                int index = currentMode == ReferenceMode.Specified ? 1 : 0;
                index = EditorGUI.Popup(modeRect, label.text, index, options);
                displayedMode = index == 1 ? ReferenceMode.Specified : ReferenceMode.Auto;
            }

            if (EditorGUI.EndChangeCheck())
            {
                mode.enumValueIndex = (int)(ReferenceMode)displayedMode;
            }

            if (mode.enumValueIndex == (int)ReferenceMode.Specified)
            {
                Rect targetRect = new Rect(
                    position.x - 15f,
                    position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    position.width + 15f,
                    EditorGUI.GetPropertyHeight(targetObject)
                );

                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(targetRect, targetObject, new GUIContent("Target Object"));
                EditorGUI.indentLevel--;

                if (!IsValid(property))
                {
                    string typeName = componentType != null ? componentType.Name : "Component";
                    Rect warningRect = new Rect(
                        position.x,
                        targetRect.yMax + EditorGUIUtility.standardVerticalSpacing,
                        position.width,
                        EditorGUIUtility.singleLineHeight * 2f
                    );
                    EditorGUI.HelpBox(warningRect, $"Missing {typeName} on Target Object!", MessageType.Error);
                }
            }
            else if (mode.enumValueIndex == (int)ReferenceMode.Variable && hasVariableCounterpart)
            {
                Rect varRect = new Rect(
                    position.x,
                    position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    position.width,
                    EditorGUIUtility.singleLineHeight
                );

                ReferenceDrawerUtils.DrawBlackboardSelector(varRect, variableName, variableType, property);
            }

            EditorGUI.EndProperty();
        }

        private Type GetComponentType()
        {
            Type type = fieldInfo.FieldType;
            if (type.IsArray) type = type.GetElementType();
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
                type = type.GetGenericArguments()[0];

            return UniversalFsmReferenceDrawer.GetGenericArgFromHierarchy(type, typeof(ComponentReference<>), 0);
        }

        private bool IsValid(SerializedProperty property)
        {
            var targetObject = property.FindPropertyRelative("targetObject");
            if (targetObject == null) return true;

            var useBlackboard = targetObject.FindPropertyRelative("useBlackboard");
            if (useBlackboard != null && useBlackboard.boolValue) return true;

            var constantValue = targetObject.FindPropertyRelative("constantValue");
            if (constantValue != null && constantValue.objectReferenceValue != null)
            {
                GameObject go = constantValue.objectReferenceValue as GameObject;
                Type componentType = GetComponentType();

                if (go && componentType != null)
                {
                    return go.GetComponent(componentType) != null;
                }
            }
            return true;
        }
    }

    [CustomPropertyDrawer(typeof(FsmReference<,>), true)]
    public class UniversalFsmReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2.3f + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Type variableType = GetVariableType();

            if (variableType != null)
            {
                ReferenceDrawerUtils.DrawReference(position, property, variableType);
            }
            else
            {
                EditorGUI.LabelField(position, "Error: Could not determine Variable Type");
            }

            EditorGUI.EndProperty();
        }

        private Type GetVariableType()
        {
            Type type = fieldInfo.FieldType;
            if (type.IsArray) type = type.GetElementType();
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                type = type.GetGenericArguments()[0];

            return GetGenericArgFromHierarchy(type, typeof(FsmReference<,>), 1);
        }

        public static Type GetGenericArgFromHierarchy(Type type, Type openGenericType, int argIndex)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType)
                {
                    return type.GetGenericArguments()[argIndex];
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}