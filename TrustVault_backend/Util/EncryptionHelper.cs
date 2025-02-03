
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TrustVault_backend.Util
{
    public static class EncryptionHelper
    {
        public static (byte[] EncryptedContent, string EncryptionKey) Encrypt(byte[] content)
        {
            using var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(content, 0, content.Length);
            cs.Close();

            var encryptedContent = ms.ToArray();
            var encryptionKey = Convert.ToBase64String(aes.Key) + ":" + Convert.ToBase64String(aes.IV);
            return (encryptedContent, encryptionKey);
        }

        public static byte[] Decrypt(byte[] encryptedContent, string encryptionKey)
        {
            var parts = encryptionKey.Split(':');
            var key = Convert.FromBase64String(parts[0]);
            var iv = Convert.FromBase64String(parts[1]);

            using var aes = Aes.Create();
            var decryptor = aes.CreateDecryptor(key, iv);

            using var ms = new MemoryStream(encryptedContent);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new MemoryStream();
            cs.CopyTo(reader);

            return reader.ToArray();
        }

    }
}
