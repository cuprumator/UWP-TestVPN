using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System;
using Windows.Security.Cryptography.DataProtection;

namespace TestVPN.Utils
{
    class Protector
    {
        public static async Task<IBuffer> Protect(string toProtect)
        {
            DataProtectionProvider Provider = new DataProtectionProvider(strDescriptor);
            IBuffer protectedBuffer = CryptographicBuffer.ConvertStringToBinary(toProtect, encoding);

            return await Provider.ProtectAsync(protectedBuffer);
        }

        public static async Task<string> Unprotect(IBuffer tounprotect)
        {
            DataProtectionProvider Provider = new DataProtectionProvider();

            IBuffer buffUnprotected = await Provider.UnprotectAsync(tounprotect);
            
            return CryptographicBuffer.ConvertBinaryToString(encoding, buffUnprotected);
        }

        private static string strDescriptor = "LOCAL=user";
        private static BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;
    }
}
