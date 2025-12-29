using UnityEngine;
using UnityEditor;
using MicheliniDev.Utils;

namespace MicheliniDev.Utils.Editor
{
    [CustomEditor(typeof(ScriptableSheet))]
    public class ScriptableSheetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ScriptableSheet sheet = (ScriptableSheet)target;
            
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14, 
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color.white } 
            };
            EditorGUILayout.LabelField($"Sheet: {sheet.name}", titleStyle);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Scriptable Sheet Data", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Rows: {sheet.rows.Count}", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"Columns: {sheet.columns.Count}", EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            GUI.backgroundColor = new Color(0.4f, 1f, 0.4f);
            if (GUILayout.Button("Open Editor Window", GUILayout.Height(40)))
            {
                ScriptableSheetWindow.Open(sheet);
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Use the Editor Window for full grid controls, search, and column configuration.", MessageType.Info);
        }
    }
}