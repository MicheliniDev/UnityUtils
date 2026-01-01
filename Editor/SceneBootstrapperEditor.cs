using MicheliniDev.Utils.ServiceLocator;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.Editor
{
    [CustomEditor(typeof(SceneBootstrapper))]
    public class SceneBootstrapperEditor : UnityEditor.Editor
    {
        private Type[] serviceInterfaces;

        private void OnEnable()
        {
            serviceInterfaces = TypeCache.GetTypesWithAttribute<LocatableAttribute>()
                .Where(t => t.IsInterface).ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(5f);

            SerializedProperty isGlobalSourceProp = serializedObject.FindProperty("isGlobalSource");
            EditorGUILayout.PropertyField(isGlobalSourceProp, new GUIContent("Is Global Source"));

            EditorGUILayout.Space();

            SerializedProperty listProp = serializedObject.FindProperty("services");

            if (GUILayout.Button("Add Service"))
            {
                listProp.InsertArrayElementAtIndex(listProp.arraySize);
                SerializedProperty entry = listProp.GetArrayElementAtIndex(listProp.arraySize - 1);

                entry.FindPropertyRelative("interfaceTypeName").stringValue = "";
                entry.FindPropertyRelative("implementation").objectReferenceValue = null;

                serializedObject.ApplyModifiedProperties();
                return;
            }

            for (int i = 0; i < listProp.arraySize; i++)
            {
                SerializedProperty entry = listProp.GetArrayElementAtIndex(i);

                SerializedProperty interfaceProp = entry.FindPropertyRelative("interfaceTypeName");
                SerializedProperty implProp = entry.FindPropertyRelative("implementation");

                EditorGUILayout.BeginVertical("box");

                Type current = string.IsNullOrEmpty(interfaceProp.stringValue)
                    ? null
                    : Type.GetType(interfaceProp.stringValue);

                string[] names = serviceInterfaces.Select(t => t.Name).ToArray();
                int index = Array.IndexOf(serviceInterfaces, current);

                int newIndex = EditorGUILayout.Popup("Interface", index, names);

                if (newIndex != index)
                {
                    interfaceProp.stringValue = serviceInterfaces[newIndex].AssemblyQualifiedName;
                    implProp.objectReferenceValue = null;
                }

                if (!string.IsNullOrEmpty(interfaceProp.stringValue))
                {
                    EditorGUILayout.PropertyField(implProp, new GUIContent("Implementation"));
                }
                UnityEngine.Object dragged = implProp.objectReferenceValue;

                if (dragged != null)
                {
                    Type iface = Type.GetType(interfaceProp.stringValue);

                    GameObject go = null;
                    Component comp = null;

                    if (dragged is GameObject g)
                    {
                        go = g;
                    }
                    else if (dragged is Component c)
                    {
                        comp = c;
                        go = c.gameObject;
                    }

                    Component resolved = null;

                    if (go != null)
                    {
                        foreach (var candidate in go.GetComponents<Component>())
                        {
                            if (iface.IsAssignableFrom(candidate.GetType()))
                            {
                                resolved = candidate;
                                break;
                            }
                        }
                    }

                    if (resolved == null)
                    {
                        EditorGUILayout.HelpBox(
                            $"{dragged.name} does NOT contain a component that implements {iface.Name}.",
                            MessageType.Error
                        );
                    }
                    else
                    {
                        implProp.objectReferenceValue = resolved;
                    }
                }

                if (GUILayout.Button("Remove"))
                {
                    listProp.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndVertical();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

}