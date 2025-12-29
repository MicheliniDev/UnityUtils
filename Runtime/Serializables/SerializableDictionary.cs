using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MicheliniDev.Utils
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializedKeyValuePair<TKey, TValue>> keyValuePairs = new();

        public new TValue this[TKey key]
        {
            get => base[key];
            set => base[key] = value;
        }

        public void OnAfterDeserialize()
        {
            Clear();

            foreach (var serializedKeyValuePair in keyValuePairs)
            {
                TryAdd(serializedKeyValuePair.Key, serializedKeyValuePair.Value);
            }
        }

        public void OnBeforeSerialize()
        {
            foreach (var originalKeyValuePair in this)
            {
                if (keyValuePairs.FirstOrDefault(value => Comparer.Equals(value.Key, originalKeyValuePair.Key))
                    is SerializedKeyValuePair<TKey, TValue> serializedKeyValuePair)
                {
                    serializedKeyValuePair.Value = originalKeyValuePair.Value;
                }
                else
                {
                    keyValuePairs.Add(originalKeyValuePair);
                }
            }

            keyValuePairs.RemoveAll(value => !ContainsKey(value.Key));
        }

        [System.Serializable]
        public class SerializedKeyValuePair<TypeKey, TypeValue>
        {
            public TypeKey Key;
            public TypeValue Value;

            public SerializedKeyValuePair(TypeKey key, TypeValue value) 
            { 
                Key = key; 
                Value = value; 
            }

            public static implicit operator SerializedKeyValuePair<TypeKey, TypeValue>(KeyValuePair<TypeKey, TypeValue> kvp)
                => new SerializedKeyValuePair<TypeKey, TypeValue>(kvp.Key, kvp.Value);
            public static implicit operator KeyValuePair<TypeKey, TypeValue>(SerializedKeyValuePair<TypeKey, TypeValue> kvp)
                => new KeyValuePair<TypeKey, TypeValue>(kvp.Key, kvp.Value);
        }
    }
}