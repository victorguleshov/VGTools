using System;

namespace VG.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAssignableTo(this Type givenType, Type anotherType)
        {
            return anotherType.IsAssignableFrom(givenType);
        }
    }
}