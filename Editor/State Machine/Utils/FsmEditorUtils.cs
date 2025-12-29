using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    public static class FsmEditorUtils
    {
        public static void DrawSerializeReferenceList(SerializedProperty listProperty, Type baseType)
        {
            listProperty.isExpanded = EditorGUILayout.Foldout(listProperty.isExpanded, listProperty.displayName, true);

            if (listProperty.isExpanded)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < listProperty.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    var element = listProperty.GetArrayElementAtIndex(i);
                    SerializedProperty nameProp = element.FindPropertyRelative("Name");
                    string variableName = "";

                    if (nameProp != null)
                        variableName = nameProp.stringValue;

                    if (string.IsNullOrEmpty(variableName))
                        variableName = baseType.Name;

                    EditorGUILayout.PropertyField(element, new GUIContent($"{variableName}"), true); 
                    EditorGUILayout.EndVertical();

                    if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        listProperty.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(2);
                }

                if (GUILayout.Button($"Add {baseType.Name}", GUILayout.Height(25)))
                {
                    ShowAddMenu(listProperty, baseType);
                }
                EditorGUI.indentLevel--;
            }
        }

        public static void DrawBlackboardSelector(Rect rect, SerializedProperty nameProp, System.Type varType, SerializedProperty rootProp)
        {
            FsmBlackboard blackboard = null;

            var target = rootProp.serializedObject.targetObject as Component;
            if (target != null)
            {
                blackboard = target.GetComponentInParent<FsmBlackboard>();
                if (blackboard == null) 
                    blackboard = target.GetComponentInChildren<FsmBlackboard>(true);
            }

            if (blackboard == null)
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

        private static void ShowAddMenu(SerializedProperty listProperty, Type baseType)
        {
            GenericMenu menu = new GenericMenu();

            var types = TypeCache.GetTypesDerivedFrom(baseType)
                .Where(t => !t.IsAbstract && !t.IsInterface).ToList();

            if (types.Count == 0)
            {
                EditorUtility.DisplayDialog(
                    "No Types Found",
                    $"Could not find any types derived from {baseType.Name}.",
                    "OK"
                );
                return;
            }

            foreach (var type in types)
            {
                string path = type.Name;
                var attr = type.GetCustomAttribute<DropdownAttribute>();
                if (attr != null)
                {
                    path = attr.MenuPath;
                }

                menu.AddItem(new GUIContent(path), false, () =>
                    CreateAndAdd(listProperty, type)
                );
            }

            menu.ShowAsContext();
        }

        private static void CreateAndAdd(SerializedProperty listProperty, Type type)
        {
            object newItem = Activator.CreateInstance(type);

            listProperty.serializedObject.Update();
            listProperty.arraySize++;
            var element = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            element.managedReferenceValue = newItem;
            listProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}