using System;
using System.Collections;

namespace Arcoro.Common.Helper
{
    public static class IEnumerableExtension
    {
        public static void Dump(this IEnumerable dumpList)
        {
            foreach (object item in dumpList)
            {
                Console.WriteLine(item.GetType());
            }
        }
    }
}