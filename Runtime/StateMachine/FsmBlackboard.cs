using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicheliniDev.Utils.FSM
{
    public class FsmBlackboard : MonoBehaviour
    {
        private Dictionary<string, FsmVariableBase> variableLookup = new Dictionary<string, FsmVariableBase>();
        private Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        [SerializeReference] public List<FsmVariableBase> Variables = new List<FsmVariableBase>();
        public List<FsmEvent> Events;

        private void Awake()
        {
            foreach (var variable in Variables)
            {
                variableLookup.Add(variable.Name, variable);
            }
        }

        public void Register<T>(T component) where T : Component
        {
            if (!components.ContainsKey(typeof(T)))
            {
                components[typeof(T)] = component;
            }
        }

        public T Get<T>(bool findInChildren = true, bool findInactive = false) where T : Component
        {
            var type = typeof(T);
            if (components.TryGetValue(type, out Component val))
                return (T)val;

            var comp = GetComponent<T>();
            if (comp)
            {
                Register(comp);
                return comp;
            }
            else if (findInChildren)
            {
                comp = GetComponentInChildren<T>(findInactive);
                Register(comp);
                return comp;
            }
            return null;
        }

        public T GetVariable<T>(string name) where T : FsmVariableBase
        {
            if (variableLookup.TryGetValue(name, out var variable))
            {
                return variable as T;
            }
            return null;
        }
    }
}
