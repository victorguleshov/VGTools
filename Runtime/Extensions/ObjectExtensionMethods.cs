using System;

namespace VG.Extensions
{
    public static class ObjectExtensionMethods
    {
        public static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            foreach (var toProperty in toProperties)
                if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                {
                    if (toProperty.CanWrite)
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                    break;
                }
        }

        public static void MatchPropertiesFrom(this object self, object parent)
        {
            var childProperties = self.GetType().GetProperties();
            foreach (var childProperty in childProperties)
            {
                var attributesForProperty = childProperty.GetCustomAttributes(typeof(MatchParentAttribute), true);
                var isOfTypeMatchParentAttribute = false;

                MatchParentAttribute currentAttribute = null;

                foreach (var attribute in attributesForProperty)
                    if (attribute.GetType() == typeof(MatchParentAttribute))
                    {
                        isOfTypeMatchParentAttribute = true;
                        currentAttribute = (MatchParentAttribute)attribute;
                        break;
                    }

                if (isOfTypeMatchParentAttribute)
                {
                    var parentProperties = parent.GetType().GetProperties();
                    object parentPropertyValue = null;
                    foreach (var parentProperty in parentProperties)
                        if (parentProperty.Name == currentAttribute.ParentPropertyName)
                            if (parentProperty.PropertyType == childProperty.PropertyType)
                                parentPropertyValue = parentProperty.GetValue(parent);
                    if (childProperty.CanWrite)
                        childProperty.SetValue(self, parentPropertyValue);
                }
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class MatchParentAttribute : Attribute
        {
            public readonly string ParentPropertyName;

            public MatchParentAttribute(string parentPropertyName)
            {
                ParentPropertyName = parentPropertyName;
            }
        }
    }
}