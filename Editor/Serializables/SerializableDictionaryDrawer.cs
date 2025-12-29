#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MicheliniDev.Utils.Editor
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        private Dictionary<string, ReorderableList> values = new Dictionary<string, ReorderableList>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                var list = GetList(property);
                list.serializedProperty = property.FindPropertyRelative("keyValuePairs");
                return list.GetHeight() + EditorGUIUtility.singleLineHeight + 6;
            }

            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.targetObject == null) return;

            Rect labelRect = position; 
            labelRect.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, label);

            if (property.isExpanded)
            {
                var list = GetList(property);
                list.serializedProperty = property.FindPropertyRelative("keyValuePairs");

                Rect listRect = position;
                listRect.y += EditorGUIUtility.singleLineHeight + 4;

                list.DoList(listRect);
            }
        }

        private ReorderableList GetList(SerializedProperty property)
        {
            string key = property.propertyPath;

            if (values.TryGetValue(key, out var list))
            {
                return list;
            }

            SerializedProperty listProp = property.FindPropertyRelative("keyValuePairs");

            list = new ReorderableList(property.serializedObject, listProp, true, true, true, true);

            list.drawHeaderCallback = (Rect rect) =>
            {
                float keyWidth = rect.width * 0.35f;
                Rect keyRect = new Rect(rect.x + 14, rect.y, keyWidth, rect.height);
                Rect valueRect = new Rect(rect.x + keyWidth + 24, rect.y, rect.width - keyWidth - 24, rect.height);

                EditorGUI.LabelField(keyRect, "Key");
                EditorGUI.LabelField(valueRect, "Value");
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index >= list.serializedProperty.arraySize) return;

                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty keyProp = element.FindPropertyRelative("Key");
                SerializedProperty valueProp = element.FindPropertyRelative("Value");

                float keyWidth = rect.width * 0.35f;
                float padding = 10f;

                Rect keyRect = new Rect(rect.x, rect.y + 2, keyWidth, EditorGUIUtility.singleLineHeight);
                Rect valueRect = new Rect(rect.x + keyWidth + padding, rect.y + 2, rect.width - keyWidth - padding, rect.height - 4);

                EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);

                bool isArray = valueProp.isArray && valueProp.propertyType != SerializedPropertyType.String;

                if (isArray)
                {
                    float yOffset = 0;

                    for (int i = 0; i < valueProp.arraySize; i++)
                    {
                        SerializedProperty arrayElement = valueProp.GetArrayElementAtIndex(i);
                        float elementHeight = EditorGUI.GetPropertyHeight(arrayElement);

                        float buttonWidth = 20f;
                        Rect elementRect = new Rect(valueRect.x, valueRect.y + yOffset, valueRect.width - buttonWidth - 2, elementHeight);
                        Rect removeButtonRect = new Rect(valueRect.x + valueRect.width - buttonWidth, valueRect.y + yOffset, buttonWidth, EditorGUIUtility.singleLineHeight);

                        EditorGUI.PropertyField(elementRect, arrayElement, GUIContent.none);

                        if (GUI.Button(removeButtonRect, new GUIContent("-", "Remove this element"), EditorStyles.miniButton))
                        {
                            valueProp.DeleteArrayElementAtIndex(i);
                            break;
                        }

                        yOffset += elementHeight + 2;
                    }

                    Rect plusButtonRect = new Rect(valueRect.x, valueRect.y + yOffset, valueRect.width, EditorGUIUtility.singleLineHeight);

                    if (GUI.Button(plusButtonRect, new GUIContent("+", "Add new element"), EditorStyles.miniButton))
                    {
                        valueProp.InsertArrayElementAtIndex(valueProp.arraySize);

                        var newElem = valueProp.GetArrayElementAtIndex(valueProp.arraySize - 1);
                        if (newElem.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            newElem.objectReferenceValue = null;
                        }
                    }
                }
                else
                {
                    EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none, true);
                }
            };

            list.elementHeightCallback = (int index) =>
            {
                if (list.serializedProperty.arraySize == 0) return 0;

                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty valueProp = element.FindPropertyRelative("Value");

                bool isArray = valueProp.isArray && valueProp.propertyType != SerializedPropertyType.String;

                if (isArray)
                {
                    float totalHeight = 0;

                    for (int i = 0; i < valueProp.arraySize; i++)
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(valueProp.GetArrayElementAtIndex(i));
                        totalHeight += 2; 
                    }

                    totalHeight += EditorGUIUtility.singleLineHeight + 4;

                    return totalHeight + 4; 
                }
                else
                {
                    if (valueProp.hasVisibleChildren) valueProp.isExpanded = true;
                    return EditorGUI.GetPropertyHeight(valueProp, true) + 6;
                }
            };

            values[key] = list;
            return list;
        }
    }
}
#endif