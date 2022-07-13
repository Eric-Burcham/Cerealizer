namespace Cerealizer
{
    using System;
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

            var fieldValue = fieldInfo!.GetValue(instance);

            return (TValue)fieldValue;
        }

        public static void InvokePrivateStaticMethod(this Type type, string methodName, params object[] arguments)
        {
            var currentType = type;
            MethodInfo methodInfo;

            do
            {
                methodInfo = currentType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                currentType = currentType.BaseType;
            }
            while (methodInfo == null && currentType != null);

            methodInfo!.Invoke(null, arguments);
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}
