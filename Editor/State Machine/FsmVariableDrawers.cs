using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    [CustomPropertyDrawer(typeof(FSM.FloatReference))]
    public class FloatReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            ReferenceDrawerUtils.DrawReference(position, property, typeof(FSM.FloatVariable));
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(FSM.IntReference))]
    public class IntReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            ReferenceDrawerUtils.DrawReference(position, property, typeof(FSM.IntVariable));
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(FSM.BoolReference))]
    public class BoolReferenceDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            ReferenceDrawerUtils.DrawReference(position, property, typeof(FSM.BoolVariable));
            EditorGUI.EndProperty();
        }
    }
}