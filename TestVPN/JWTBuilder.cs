using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TestVPN
{
    class JWTBuilder
    {
        public static string Build(string payload, string secret)
        {
            HMACSHA256 hash = new HMACSHA256(Encoding.UTF8.GetBytes(secret));

            string encodedHeader = Base64Encoder.Encode(header);
            string encodedPayload = "eyJuYW1lIjoiSm9obiBEb2UifQ";//Base64Encoder.Encode(payload);

            string signature = Base64Encoder.Encode(hash.ComputeHash(Encoding.UTF8.GetBytes(encodedHeader + "." +  encodedPayload)));

            return encodedHeader + "." + encodedPayload + "." + signature;
        }

        private static readonly string header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
    }
}
