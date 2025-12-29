using UnityEngine;
using UnityEditor;

namespace MicheliniDev.Utils.Editor
{
    public class ScriptableSheetWindow : EditorWindow
    {
        private ScriptableSheet currentSheet;
        private Vector2 scrollPos;
        private string searchQuery = "";

        [MenuItem("Window/Scriptable Sheet Editor")]
        public static void ShowWindow()
        {
            GetWindow<ScriptableSheetWindow>("Sheet Editor");
        }

        public static void Open(ScriptableSheet sheet)
        {
            ScriptableSheetWindow wnd = GetWindow<ScriptableSheetWindow>("Sheet Editor");
            wnd.currentSheet = sheet;
            wnd.Show();
        }

        private void OnGUI()
        {
            if (!currentSheet)
            {
                DrawEmptyState();
                return;
            }

            DrawHeader();
            DrawToolbar();
            DrawGrid();

            if (GUI.changed)
            {
                currentSheet.ValidateData();
                EditorUtility.SetDirty(currentSheet);
            }
        }

        private void DrawEmptyState()
        {
            EditorGUILayout.HelpBox("No Scriptable Sheet selected.", MessageType.Info);
            currentSheet =
                (ScriptableSheet)EditorGUILayout.ObjectField("Select Sheet", currentSheet, typeof(ScriptableSheet),
                    false);
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.LabelField($"Editing: {currentSheet.name}", EditorStyles.boldLabel);
            if (GUILayout.Button("Ping Asset", EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                EditorGUIUtility.PingObject(currentSheet);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Add Column", EditorStyles.toolbarButton))
            {
                Undo.RecordObject(currentSheet, "Add Column");
                currentSheet.columns.Add(new ScriptableSheet.ColumnDefinition());
                currentSheet.ValidateData();
            }

            if (GUILayout.Button("Add Row", EditorStyles.toolbarButton))
            {
                Undo.RecordObject(currentSheet, "Add Row");
                currentSheet.rows.Add(new ScriptableSheet.RowData() { key = "NewKey" });
                currentSheet.ValidateData();
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField("Search:", GUILayout.Width(50));
            searchQuery = EditorGUILayout.TextField(searchQuery, EditorStyles.toolbarSearchField, GUILayout.Width(200));

            EditorGUILayout.EndHorizontal();
        }

        private void DrawGrid()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("KEY", new GUIStyle()
            {
                richText = true,
                fontSize = 14,
                alignment = TextAnchor.LowerCenter,
                normal =
                {
                    textColor = Color.white
                },
                padding =
                {
                    top = 30
                }
            }, GUILayout.Width(120));
            
            for (int i = 0; i < currentSheet.columns.Count; i++)
            {
                var col = currentSheet.columns[i];
                
                if (GUILayout.Button($"{col.header}\n<color=grey>({col.type})</color>", new GUIStyle(EditorStyles.miniButtonMid)
                    {
                        richText = true, 
                        alignment = TextAnchor.MiddleCenter,
                        fixedHeight = 40f
                    }, GUILayout.Width(col.width), GUILayout.Height(30)))
                {
                    Rect btnRect = GUILayoutUtility.GetLastRect();
                    var center = btnRect.center;
                    center.x += 70f + (150f * i);
                    btnRect.center = center;
                    PopupWindow.Show(btnRect, new ColumnHeaderPopup(currentSheet, i, this));
                }
            }
            
            GUILayout.Label("", GUILayout.Width(30)); 
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            for (int r = 0; r < currentSheet.rows.Count; r++)
            {
                var row = currentSheet.rows[r];

                if (!string.IsNullOrEmpty(searchQuery) && !row.key.ToLower().Contains(searchQuery.ToLower()))
                    continue;

                EditorGUILayout.BeginHorizontal();

                Undo.RecordObject(currentSheet, "Edit Key");
                row.key = EditorGUILayout.TextField(row.key, GUILayout.Width(120));

                for (int c = 0; c < currentSheet.columns.Count; c++)
                {
                    if (c >= row.values.Count) continue;

                    var colDef = currentSheet.columns[c];
                    string currentVal = row.values[c];
                    string newVal = currentVal;

                    GUILayoutOption widthOpt = GUILayout.Width(97.5f);

                    Undo.RecordObject(currentSheet, "Edit Cell");

                    switch (colDef.type)
                    {
                        case MicheliniDev.Utils.VariableType.String: 
                            newVal = EditorGUILayout.TextField(currentVal, widthOpt);
                            break;

                        case MicheliniDev.Utils.VariableType.Int:
                            int.TryParse(currentVal, out int intVal);
                            newVal = EditorGUILayout.IntField(intVal, widthOpt).ToString();
                            break;

                        case MicheliniDev.Utils.VariableType.Float:
                            float.TryParse(currentVal, out float floatVal);
                            newVal = EditorGUILayout.FloatField(floatVal, widthOpt).ToString();
                            break;

                        case MicheliniDev.Utils.VariableType.Bool:
                            bool.TryParse(currentVal, out bool boolVal);
                            newVal = EditorGUILayout.Toggle(boolVal, widthOpt).ToString();
                            break;
                    }

                    row.values[c] = newVal;
                }

                GUI.backgroundColor = new Color(1f, 0.5f, 0.5f); 
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    Undo.RecordObject(currentSheet, "Remove Row");
                    currentSheet.rows.RemoveAt(r);
                }
                GUI.backgroundColor = Color.white;

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
    }

    public class ColumnHeaderPopup : PopupWindowContent
    {
        private ScriptableSheet _sheet;
        private int _colIndex;
        private ScriptableSheetWindow _editorWindow;

        public ColumnHeaderPopup(ScriptableSheet sheet, int colIndex, ScriptableSheetWindow editorWindow)
        {
            _sheet = sheet;
            _colIndex = colIndex;
            _editorWindow = editorWindow;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 130);
        }

        public override void OnGUI(Rect rect)
        {
            if (_sheet == null || _colIndex >= _sheet.columns.Count)
            {
                editorWindow.Close();
                return;
            }

            var col = _sheet.columns[_colIndex];

            GUILayout.Label($"Edit Column: {col.header}", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(_sheet, "Modify Column Config");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name:", GUILayout.Width(50));
            col.header = EditorGUILayout.TextField(col.header);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type:", GUILayout.Width(50));
            col.type = (MicheliniDev.Utils.VariableType)EditorGUILayout.EnumPopup(col.type);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Width:", GUILayout.Width(50));
            col.width = EditorGUILayout.FloatField(col.width);
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                _sheet.ValidateData(); 
                _editorWindow.Repaint(); 
            }

            EditorGUILayout.Space();
            
            GUI.backgroundColor = new Color(1f, 0.6f, 0.6f);
            if (GUILayout.Button("Remove Column"))
            {
                Undo.RecordObject(_sheet, "Remove Column");
                _sheet.columns.RemoveAt(_colIndex);
                _sheet.ValidateData();
                _editorWindow.Repaint();
                editorWindow.Close(); 
            }
            GUI.backgroundColor = Color.white;
        }
    }
}