using System;

namespace ArcoroSamples.common
{
    public static class EnumExtension
    {
        public static void Dump<T>(this T enumerator) where T : Enum
        {
            Console.WriteLine(enumerator.ToString());
        }
    }
}