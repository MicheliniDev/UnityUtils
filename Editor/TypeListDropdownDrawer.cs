using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.Editor
{
    [CustomPropertyDrawer(typeof(TypeDropdownAttribute))]
    public class TypeListDropdownDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isArray)
            {
                if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;

                float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; 

                for (int i = 0; i < property.arraySize; i++)
                {
                    var element = property.GetArrayElementAtIndex(i);
                    float elementHeight = EditorGUI.GetPropertyHeight(element, true);

                    if (elementHeight < EditorGUIUtility.singleLineHeight)
                        elementHeight = EditorGUIUtility.singleLineHeight;

                    height += elementHeight + EditorGUIUtility.standardVerticalSpacing + 6;
                }

                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2; 
                return height;
            }

            if (property.managedReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;

            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            TypeDropdownAttribute typeAttribute = attribute as TypeDropdownAttribute;
            Type baseType = typeAttribute?.BaseType;

            if (baseType == null)
            {
                EditorGUI.LabelField(position, label.text, "Error: BaseType is null in Attribute");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            if (property.isArray)
                DrawList(position, property, label, baseType);
            else
                DrawSingle(position, property, label, baseType);

            EditorGUI.EndProperty();
        }

        private void DrawList(Rect position, SerializedProperty property, GUIContent label, Type baseType)
        {
            Rect headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                Rect currentRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, 0);

                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(i);
                    float elementHeight = EditorGUI.GetPropertyHeight(element, true);
                    if (elementHeight < EditorGUIUtility.singleLineHeight) elementHeight = EditorGUIUtility.singleLineHeight;

                    float boxHeight = elementHeight + 6;
                    currentRect.height = boxHeight;

                    Rect boxRect = EditorGUI.IndentedRect(currentRect);
                    GUI.Box(boxRect, GUIContent.none, EditorStyles.helpBox);

                    Rect propRect = new Rect(boxRect.x + 4, boxRect.y + 3, boxRect.width - 25, elementHeight);

                    string typeLabel = GetTypeName(element);
                    EditorGUI.PropertyField(propRect, element, new GUIContent(typeLabel), true);

                    Rect removeRect = new Rect(boxRect.xMax - 22, boxRect.y + 2, 20, 18);

                    if (GUI.Button(removeRect, "X"))
                    {
                        property.DeleteArrayElementAtIndex(i);
                        break;
                    }

                    currentRect.y += boxHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                Rect btnRect = new Rect(position.x + (EditorGUI.indentLevel * 15), currentRect.y + 2, position.width - (EditorGUI.indentLevel * 15), EditorGUIUtility.singleLineHeight);
                if (GUI.Button(btnRect, $"Add {baseType.Name}"))
                {
                    ShowAddMenu(property, baseType, -1);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawSingle(Rect position, SerializedProperty property, GUIContent label, Type baseType)
        {
            if (property.managedReferenceValue == null)
            {
                Rect btnRect = EditorGUI.PrefixLabel(position, label);
                if (GUI.Button(btnRect, $"Select {baseType.Name}"))
                {
                    ShowAddMenu(property, baseType, 0);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private string GetTypeName(SerializedProperty element)
        {
            if (element.managedReferenceValue == null) return "Null";

            SerializedProperty nameProp = element.FindPropertyRelative("Name");
            if (nameProp != null && !string.IsNullOrEmpty(nameProp.stringValue))
            {
                return nameProp.stringValue;
            }

            var type = element.managedReferenceValue.GetType();
            return type.Name;
        }

        private void ShowAddMenu(SerializedProperty property, Type baseType, int listIndex)
        {
            GenericMenu menu = new GenericMenu();

            var types = TypeCache.GetTypesDerivedFrom(baseType)
                .Where(t => !t.IsAbstract && !t.IsInterface && !t.IsGenericType)
                .OrderBy(t => t.Name);

            if (!types.Any())
            {
                menu.AddDisabledItem(new GUIContent($"No types found derived from {baseType.Name}"));
                menu.ShowAsContext();
                return;
            }

            foreach (var type in types)
            {
                string path = type.Name;

                var attr = type.GetCustomAttribute<DropdownAttribute>();
                if (attr != null && !string.IsNullOrEmpty(attr.MenuPath))
                {
                    path = attr.MenuPath;
                }

                menu.AddItem(new GUIContent(path), false, () =>
                {
                    ApplyType(property, type, listIndex);
                });
            }

            if (listIndex != -1)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("None"), false, () =>
                {
                    property.serializedObject.Update();
                    property.managedReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.ShowAsContext();
        }

        private void ApplyType(SerializedProperty property, Type type, int listIndex)
        {
            object newItem = Activator.CreateInstance(type);
            property.serializedObject.Update();

            if (listIndex == -1)
            {
                property.arraySize++;
                var element = property.GetArrayElementAtIndex(property.arraySize - 1);
                element.managedReferenceValue = newItem;
            }
            else
            {
                property.managedReferenceValue = newItem;
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}