using System;

namespace ArcoroSamples.common
{
    public static class StringExtension
    {
        public static void Dump(this string message)
        {
            Console.WriteLine(message);
        }
        
        public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : Enum 
            => !Enum.IsDefined(typeof(T), value) ? 
                throw new Exception($"Value {value} of enum {typeof(T).Name} is not supported") : 
                (T) Enum.Parse(typeof(T), value, ignoreCase);
    }
    
}