using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MicheliniDev.Utils.ScriptableVariables
{
    public abstract class ScriptableVariable<T> : ScriptableObject
    {
        private enum ResetMode
        {
            OnSceneLoad,
            PlayModeStart,
            Never,
        }
        
        [SerializeField] private ResetMode mode = ResetMode.PlayModeStart;

        private T _value;
        public T InitialValue;
        
        public event Action<T> OnValueChanged;
        
        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value)) return;
                
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        private void OnEnable()
        {
            if (mode == ResetMode.PlayModeStart)
                OnReset();
            
            if (mode == ResetMode.OnSceneLoad)
                SceneManager.sceneLoaded += ResetOnSceneLoad;
        }

        private void OnDisable()
        {
            if (mode == ResetMode.OnSceneLoad)
                SceneManager.sceneLoaded -= ResetOnSceneLoad;
        }

        private void ResetOnSceneLoad(Scene scene, LoadSceneMode mode) => OnReset();
        
        public void OnReset()
        {
            Value = InitialValue;
        }
    }
}