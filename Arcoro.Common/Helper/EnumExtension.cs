using System;

namespace Arcoro.Common.Helper
{
    public static class EnumExtension
    {
        public static void Dump<T>(this T enumerator) where T : Enum
        {
            Console.WriteLine(enumerator.ToString());
        }
    }
}