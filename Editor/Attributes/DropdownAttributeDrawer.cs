using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using VG.Attributes;

namespace VG.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var options = GetOptions(property);
            DrawDropdown(position, property, label, options);

            EditorGUI.EndProperty();
        }

        private string[] GetOptions(SerializedProperty property)
        {
            var attr = attribute as DropdownAttribute;
            var methodName = attr.MethodName;

            var objectType = fieldInfo.DeclaringType;

            var methodOwnerType = attr.Location == DropdownAttribute.MethodLocation.PropertyClass ? objectType : attr.MethodOwnerType;

            var methodInfo = methodOwnerType.GetMethod
            (methodName,
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.Instance);

            if (methodInfo == null)
            {
                Debug.LogError($"Method {methodName} In {methodOwnerType.FullName} Could Not Be Found!");
                return new[] { "<error: method not found>" };
            }

            var methodInfoReturnValueIsStringArray = methodInfo.ReturnType == typeof(string[]);
            if (!methodInfoReturnValueIsStringArray)
            {
                Debug.LogError($"Method {methodName} In {methodOwnerType.FullName} Does Not Have A Return Type Of {typeof(string[]).FullName}");
                return new[] { "<error: invalid return value>" };
            }

            var invokeReference = attr.Location == DropdownAttribute.MethodLocation.StaticClass ? null : property.serializedObject.targetObject;

            var returnValue = methodInfo.Invoke(invokeReference, null) as string[];

            return returnValue;
        }

        private void DrawDropdown(Rect position, SerializedProperty property, GUIContent label, string[] options)
        {
            if (options == null || options.Length == 0)
            {
                options = new[] { "<error>" };
            }

            var selectedIndex = Array.IndexOf(options, property.stringValue);
            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                selectedIndex = EditorGUI.Popup(EditorGUI.PrefixLabel(position, label), selectedIndex, options);
                if (check.changed)
                {
                    property.stringValue = options[selectedIndex];
                }
            }
        }
    }
}