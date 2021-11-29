using System;
using System.Collections.Generic;

namespace ArcoroSamples.common
{
    public static class Util
    {
        private static string ApiKey => Environment.GetEnvironmentVariable("SAGE_APIKEY");
        private static string ApiSecret => Environment.GetEnvironmentVariable("SAGE_SECRET");
        private static string UserName => Environment.GetEnvironmentVariable("SAGE_USER");
        private static string UserPassword => Environment.GetEnvironmentVariable("SAGE_USER_PASSWORD");

        private static Dictionary<string, string> SecretValues => new Dictionary<string, string>
        {
            {"hh2apikey", ApiKey},
            {"hh2apisecret", ApiSecret},
            {"hh2username", UserName},
            {"arcoro_test_300", UserPassword}
        };
        public static string GetPassword(string key)
        {
            if(SecretValues.ContainsKey(key)) return SecretValues[key];
            throw new Exception("Element doesn't exist");
        }
        
        public static string RawHtml(string key)
        {
            return "";
        }
    }
}