using System.Collections.Generic;

namespace Cerealizer;

internal static class Extensions
{
    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || list.Count == 0;
    }
}