using System;
using System.Collections.Generic;
using System.Reflection;

namespace ArcoroSamples.common
{
    public static class ListExtension
    {
        public static void Dump<T>(this List<T> dumpList) where T : class
        {
            foreach (var item in dumpList)
            {
                var output = "";
                foreach (var propertyInfo in item.GetPropertyList())
                {
                    output += $"{propertyInfo.Name} -> {propertyInfo.GetValue(propertyInfo.Name)};";
                }
                Console.WriteLine(output);
            }
        }

        private static PropertyInfo[] GetPropertyList<T> (this T instance) where T : class
        {
            return instance.GetType().GetProperties();
        }
    }
}