namespace Cerealizer
{
    using System.Collections.Generic;

    internal static class Extensions
    {
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}
