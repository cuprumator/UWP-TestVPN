using System.Text;
using System.Security.Cryptography;

namespace TestVPN.Utils
{
    class JWTBuilder
    {
        public static string Build(string payload, string secret)
        {
            var hash = new HMACSHA256(Encoding.UTF8.GetBytes(secret));

            string encodedHeader = Base64Encoder.Encode(Header);
            string encodedPayload = Base64Encoder.Encode(payload);

            string signature = Base64Encoder.Encode(hash.ComputeHash(Encoding.UTF8.GetBytes(encodedHeader + "." +  encodedPayload)));

            return encodedHeader + "." + encodedPayload + "." + signature;
        }

        private static readonly string Header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
    }
}
