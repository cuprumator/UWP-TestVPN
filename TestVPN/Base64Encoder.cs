using System;
using System.Text;

namespace TestVPN
{
    class Base64Encoder
    {
        public static string Encode(string input)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(input);
            return ToBase64(plainTextBytes);
        }

        public static string Encode(byte[] input)
        {
            return ToBase64(input);
        }

        private static string ToBase64(byte[] input)
        {
            return Convert.ToBase64String(input).TrimEnd(padding).Replace('+', '-').Replace('/', '_'); ;
        }

        private static readonly char[] padding = { '=' };
    }
}
