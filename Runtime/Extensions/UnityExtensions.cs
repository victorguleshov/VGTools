namespace VG.Extensions
{
    public static class UnityExtensions
    {
        // ReSharper disable once CompareNonConstrainedGenericWithNull
        public static bool IsUnityNull<T>(this T obj)
        {
            return obj == null || obj.Equals(null);
        }
    }
}