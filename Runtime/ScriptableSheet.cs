using System.Collections.Generic;
using UnityEngine;

namespace MicheliniDev.Utils
{
    [CreateAssetMenu(menuName = "Michelini Utils/Scriptable Sheet", fileName = "Scriptable Sheet")]
    public class ScriptableSheet : ScriptableObject, ISerializationCallbackReceiver
    {
        [System.Serializable]
        public class ColumnDefinition
        {
            public string header = "New Column";
            public VariableType type = VariableType.String;
            public float width = 100f; 
        }

        [System.Serializable]
        public class RowData
        {
            public string key;
            public List<string> values = new List<string>(); 
        }

        public List<ColumnDefinition> columns = new List<ColumnDefinition>();
        public List<RowData> rows = new List<RowData>();

        private Dictionary<string, List<string>> dataLookup = new Dictionary<string, List<string>>();
        private Dictionary<string, int> headerLookup = new Dictionary<string, int>();

        private Dictionary<string, float> floatCache = new Dictionary<string, float>();
        private Dictionary<string, int> intCache = new Dictionary<string, int>();
        private Dictionary<string, bool> boolCache = new Dictionary<string, bool>();

        public void OnAfterDeserialize() { InitializeLookup(); }
        public void OnBeforeSerialize() { }
    
        public void ValidateData()
        {
            foreach (var row in rows)
            {
                while (row.values.Count < columns.Count) row.values.Add("");
                while (row.values.Count > columns.Count) row.values.RemoveAt(row.values.Count - 1);
            }
        }

        private void InitializeLookup()
        {
            dataLookup.Clear();
            headerLookup.Clear();

            floatCache.Clear();
            intCache.Clear();
            boolCache.Clear();

            for (int i = 0; i < columns.Count; i++)
                headerLookup[columns[i].header] = i;

            foreach (var row in rows)
            {
                if (!string.IsNullOrEmpty(row.key) && !dataLookup.ContainsKey(row.key))
                    dataLookup.Add(row.key, row.values);
            }
        }

        private string GetCacheKey(string rowKey, string colName) => rowKey + "_" + colName;

        public string GetString(string rowKey, string colName)
        {
            return GetRaw(rowKey, colName);
        }

        public float GetFloat(string rowKey, string colName, float defaultVal = 0)
        {
            string cacheKey = GetCacheKey(rowKey, colName);

            if (floatCache.TryGetValue(cacheKey, out float cachedResult))
                return cachedResult;

            string val = GetRaw(rowKey, colName);

            if (float.TryParse(val, out float result))
            {
                floatCache[cacheKey] = result;
                return result;
            }

            return defaultVal;
        }

        public int GetInt(string rowKey, string colName, int defaultVal = 0)
        {
            string cacheKey = GetCacheKey(rowKey, colName);

            if (intCache.TryGetValue(cacheKey, out int cachedResult))
                return cachedResult;

            string val = GetRaw(rowKey, colName);

            if (int.TryParse(val, out int result))
            {
                intCache[cacheKey] = result;
                return result;
            }

            return defaultVal;
        }

        public bool GetBool(string rowKey, string colName, bool defaultVal = false)
        {
            string cacheKey = GetCacheKey(rowKey, colName);

            if (boolCache.TryGetValue(cacheKey, out bool cachedResult))
                return cachedResult;

            string val = GetRaw(rowKey, colName);

            if (bool.TryParse(val, out bool result))
            {
                boolCache[cacheKey] = result;
                return result;
            }

            return defaultVal;
        }

        private string GetRaw(string rowKey, string colName)
        {
            if (headerLookup.TryGetValue(colName, out int colIndex))
            {
                if (dataLookup.TryGetValue(rowKey, out List<string> rowValues) &&
                    colIndex < rowValues.Count)
                {
                    return rowValues[colIndex];
                }
            }
            return string.Empty;
        }
    }
}