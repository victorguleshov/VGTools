using System;
using UnityEngine;

namespace VG.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DropdownAttribute : PropertyAttribute
    {
        public enum MethodLocation
        {
            PropertyClass,
            StaticClass
        }

        public DropdownAttribute(string methodName)
        {
            Location = MethodLocation.PropertyClass;
            MethodName = methodName;
        }

        public DropdownAttribute(Type methodOwner, string methodName)
        {
            Location = MethodLocation.StaticClass;
            MethodOwnerType = methodOwner;
            MethodName = methodName;
        }

        public MethodLocation Location { get; private set; }
        public string MethodName { get; private set; }
        public Type MethodOwnerType { get; private set; }
    }
}