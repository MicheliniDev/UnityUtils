#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.Editor
{
    [CustomPropertyDrawer(typeof(SerializableInterface<>))]
    public class SerializableInterfaceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            label.text = string.Empty;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            SerializedProperty targetProp = property.FindPropertyRelative("targetObject");

            Type concreteType = GetTypeFromProperty(property);
            Type interfaceType = typeof(UnityEngine.Object);


            if (concreteType != null && concreteType.IsGenericType)
            {
                interfaceType = concreteType.GetGenericArguments()[0];
            }

            EditorGUI.BeginChangeCheck();

            UnityEngine.Object newObject = EditorGUI.ObjectField(
                position,
                GUIContent.none,
                targetProp.objectReferenceValue,
                typeof(UnityEngine.Object),
                true
            );

            if (EditorGUI.EndChangeCheck())
            {
                UnityEngine.Object validatedObject = null;

                if (newObject != null)
                {
                    if (newObject is GameObject go)
                    {
                        validatedObject = go.GetComponent(interfaceType);
                    }
                    else if (interfaceType.IsAssignableFrom(newObject.GetType()))
                    {
                        validatedObject = newObject;
                    }
                    else if (newObject is Component comp)
                    {
                        validatedObject = comp.GetComponent(interfaceType);
                    }
                }

                targetProp.objectReferenceValue = validatedObject;
                property.serializedObject.ApplyModifiedProperties();
            }

            if (newObject == null && Event.current.type == EventType.Repaint) 
            { 
                GUIStyle style = new GUIStyle(EditorStyles.objectField);
                style.fontSize = EditorStyles.label.fontSize;
                style.alignment = TextAnchor.MiddleLeft;

                Rect overlayRect = position;
                overlayRect.width -= 20;

                GUI.Label(overlayRect, $"None ({interfaceType.Name})", style);
            }
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private Type GetTypeFromProperty(SerializedProperty property)
        {
            Type parentType = property.serializedObject.targetObject.GetType();
            string[] pathParts = property.propertyPath.Split('.');

            for (int i = 0; i < pathParts.Length; i++)
            {
                string part = pathParts[i];

                if (part == "Array" && i + 1 < pathParts.Length && pathParts[i + 1].StartsWith("data["))
                {
                    if (parentType.IsArray)
                    {
                        parentType = parentType.GetElementType();
                    }
                    else if (parentType.IsGenericType && parentType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
                    {
                        parentType = parentType.GetGenericArguments()[0];
                    }

                    i++;
                }
                else
                {
                    FieldInfo fi = parentType.GetField(part, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                    if (fi == null) return null;

                    parentType = fi.FieldType;
                }
            }
            return parentType;
        }
    }
}
#endif