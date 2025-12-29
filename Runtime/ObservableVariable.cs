using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace MicheliniDev.Utils
{
    [Serializable]
    public class ObservableVariable<T>
    {
        [SerializeField] private T currentValue;
        [SerializeField] private UnityEvent<T, T> OnValueChanged;
        private T previousValue;

        public T Value
        {
            get => currentValue;
            set => Set(value);
        }

        public void Set(T value)
        {
            if (Equals(currentValue, value)) return;

            previousValue = currentValue;
            currentValue = value;
            
            Invoke();
        }

        public ObservableVariable(T value = default, UnityAction<T, T> callback = null)
        {
            currentValue = value;
            
            OnValueChanged = new UnityEvent<T, T>();
            
            if (callback != null) 
                OnValueChanged.AddListener(callback);
        }

        public void Invoke()
        {
            OnValueChanged?.Invoke(previousValue, currentValue);
        }

        public void Validate()
        {
            if (!Equals(currentValue, previousValue))
            {
                Invoke();
                previousValue = currentValue;
            }
        }

        public void AddListener(UnityAction<T, T> callback)
        {
            if (callback == null) return;
            
            OnValueChanged ??= new UnityEvent<T, T>();
            OnValueChanged.AddListener(callback);
        }

        public void RemoveListener(UnityAction<T, T> callback)
        {
            if (callback == null) return;
            if (OnValueChanged == null) return;

            OnValueChanged.RemoveListener(callback);
        }

        public void RemoveAllListeners()
        {
            if (OnValueChanged == null) return;

            OnValueChanged.RemoveAllListeners();
        }

        public static implicit operator T(ObservableVariable<T> observer) => observer.currentValue;
    }
}