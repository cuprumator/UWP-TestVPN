
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
            //             IBuffer inputBuffer = CryptographicBuffer.ConvertStringToBinary(toProtect, BinaryStringEncoding.Utf8);
            //             BufferProtectUnprotectResult procBuffer = await DataProtectionManager.ProtectAsync(inputBuffer, identity);
            //             inputBuffer = procBuffer.Buffer;
            //             return CryptographicBuffer.EncodeToHexString(inputBuffer).Substring(0, 20);

            DataProtectionProvider Provider = new DataProtectionProvider(strDescriptor);
            IBuffer protectedBuffer = CryptographicBuffer.ConvertStringToBinary(toProtect, encoding);

           return await Provider.ProtectAsync(protectedBuffer);
           // return CryptographicBuffer.EncodeToHexString(buffProtected).Substring(0, 20);

        }

        public static async Task<string> Unprotect(IBuffer tounprotect)
        {
            DataProtectionProvider Provider = new DataProtectionProvider();

            //IBuffer protectedBuffer = CryptographicBuffer.ConvertStringToBinary(tounprotect, encoding);

            IBuffer buffUnprotected = await Provider.UnprotectAsync(tounprotect);
            
            return CryptographicBuffer.ConvertBinaryToString(encoding, buffUnprotected);
        }

        private static string strDescriptor = "LOCAL=user";
        private static BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;
    }
}
