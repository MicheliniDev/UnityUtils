using System;

namespace MicheliniDev.Utils
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DropdownAttribute : Attribute
    {
        public string MenuPath { get; private set; }

        public DropdownAttribute(string menuPath)
        {
            MenuPath = menuPath;
        }
    }
}