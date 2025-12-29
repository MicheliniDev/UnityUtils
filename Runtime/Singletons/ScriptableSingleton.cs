using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MicheliniDev.Utils
{
    public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance)
                    return instance;

                instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                if (instance) 
                    return instance;

#if UNITY_EDITOR
                string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    instance = AssetDatabase.LoadAssetAtPath<T>(path);
                    return instance;
                }
#endif

                instance = Resources.Load<T>(typeof(T).Name);
                if (instance) 
                    return instance;

                if (!instance)
                {
                    instance = CreateInstance<T>();
#if UNITY_EDITOR
                    CreateAssetFile(instance);
#endif
                }

                return instance;
            }
        }

#if UNITY_EDITOR
        private static void CreateAssetFile(T obj)
        {
            string folderPath = "Assets/Resources/ScriptableSingletons";
            string fileName = $"{typeof(T).Name}.asset";
            string fullPath = $"{folderPath}/{fileName}";

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            if (!AssetDatabase.IsValidFolder("Assets/Resources/ScriptableSingletons"))
                AssetDatabase.CreateFolder("Assets/Resources", "ScriptableSingletons");

            Debug.Log($"<color=green>Creating ScriptableSingleton at: {fullPath}</color>");

            AssetDatabase.CreateAsset(obj, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}