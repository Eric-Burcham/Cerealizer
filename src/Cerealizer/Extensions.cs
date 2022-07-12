namespace Cerealizer
{
    using System.Collections.Generic;
    using System.Reflection;

    internal static class Extensions
    {
        public static TValue GetPrivateFieldValue<TValue>(this object instance, string fieldName)
        {
            var instanceType = instance.GetType();
            var currentType = instanceType;
            FieldInfo fieldInfo;

            do
            {
                fieldInfo = currentType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                currentType = currentType.BaseType;
            }
            while (fieldInfo == null && currentType != null);

            var fieldValue = fieldInfo.GetValue(instance);

            return (TValue)fieldValue;
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}
