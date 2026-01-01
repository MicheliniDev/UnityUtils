using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [CustomPropertyDrawer(typeof(FsmEvent))]
    public class FsmEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty idProp = property.FindPropertyRelative("id");
            string currentId = idProp.stringValue;

            FsmBlackboard blackboard = null;
            var targetObject = property.serializedObject.targetObject;

            if (targetObject is Component component)
            {
                blackboard = component.GetComponentInParent<FsmBlackboard>();
                if (blackboard == null) blackboard = component.GetComponentInChildren<FsmBlackboard>(true);
            }

            if (blackboard == null || blackboard.Events == null)
            {
                string newValue = EditorGUI.TextField(position, label, currentId);
                if (newValue != currentId) idProp.stringValue = newValue;
            }
            else
            {
                var options = new List<string> { "None" };
                options.AddRange(blackboard.Events
                    .Where(x => x != null && !string.IsNullOrEmpty(x.Id))
                    .Select(x => x.Id));

                int index = string.IsNullOrEmpty(currentId) ? 0 : options.IndexOf(currentId);

                if (index == -1 && !string.IsNullOrEmpty(currentId))
                {
                    options.Add($"{currentId} [MISSING]");
                    index = options.Count - 1;
                }

                int newIndex = EditorGUI.Popup(position, label.text, index, options.ToArray());

                if (newIndex != index)
                {
                    if (newIndex >= 0 && newIndex < options.Count)
                    {
                        string selected = options[newIndex];
                        if (!selected.Contains("[MISSING]"))
                        {
                            idProp.stringValue = (newIndex == 0) ? "" : selected;
                        }
                    }
                }
            }

            EditorGUI.EndProperty();
        }
    }
}