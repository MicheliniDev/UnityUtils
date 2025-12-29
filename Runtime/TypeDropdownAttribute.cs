using System;
using UnityEngine;

namespace MicheliniDev.Utils
{
    public class TypeDropdownAttribute : PropertyAttribute
    {
        public Type BaseType { get; private set; }

        public TypeDropdownAttribute(Type baseType)
        {
            BaseType = baseType;
        }
    }
}