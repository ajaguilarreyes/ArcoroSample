using System;
using System.Collections;

namespace ArcoroSamples.common
{
    public static class IEnumerableExtension
    {
        public static void Dump(this IEnumerable dumpList)
        {
            foreach (var item in dumpList)
            {
                Console.WriteLine(item.GetType());
            }
        }
    }
}