using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MicheliniDev.Utils.EventBus.Editor
{
    [CustomPropertyDrawer(typeof(EventBusAttribute))]
    public class EventBusAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, "Use [EventBus] with strings only.");
                return;
            }

            var guids = AssetDatabase.FindAssets("t:GlobalBusEvents");
            if (guids.Length == 0)
            {
                EditorGUI.LabelField(position, label.text, "GlobalBusEvents asset not found.");
                return;
            }

            var eventsSO = AssetDatabase.LoadAssetAtPath<GlobalBusEvents>(AssetDatabase.GUIDToAssetPath(guids[0]));
            if (eventsSO == null || eventsSO.EventNames == null) return;

            List<string> options = new List<string>(eventsSO.EventNames);

            int index = options.IndexOf(property.stringValue);
            if (index == -1) 
                index = 0;

            index = EditorGUI.Popup(position, label.text, index, options.ToArray());

            if (index >= 0 && index < options.Count)
            {
                property.stringValue = options[index];
            }
        }
    }
}