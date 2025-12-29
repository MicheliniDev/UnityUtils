using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [CustomPropertyDrawer(typeof(FsmBlackboardEventAttribute))]
    public class FsmBlackboardEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            FsmBlackboard blackboard = null;
            var component = property.serializedObject.targetObject as Component;
            if (component != null)
            {
                blackboard = component.GetComponentInParent<FsmBlackboard>();
                if (blackboard == null) blackboard = component.GetComponentInChildren<FsmBlackboard>(true);
            }

            if (blackboard == null || blackboard.Events == null || blackboard.Events.Count == 0)
            {
                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

                position.width += 14f;
                position.x -= 14.5f;

                var eventList = blackboard.Events.Where(id => !string.IsNullOrEmpty(id)).ToList();
                int index = eventList.IndexOf(property.stringValue);

                int newIndex = EditorGUI.Popup(position, index, eventList.ToArray());

                if (newIndex >= 0 && newIndex < eventList.Count)
                {
                    property.stringValue = eventList[newIndex];
                }
            }
            EditorGUI.EndProperty();
        }
    }
}
