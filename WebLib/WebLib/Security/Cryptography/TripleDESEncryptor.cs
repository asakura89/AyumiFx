using System;
using System.Security.Cryptography;
using System.Text;

namespace WebLib.Security.Cryptography
{
    public class TripleDESEncryptor : IDecryptable
    {
        private const String InternalKey = "5w0Rd4r70nL1n3";
        private readonly Byte[] keyArray;

        public TripleDESEncryptor()
        {
            MD5Encryptor encryptor = new MD5Encryptor();
            keyArray = encryptor.GetHashBytes(InternalKey);
        }

        public String Encrypt(String stringToEncrypt)
        {
            using (TripleDESCryptoServiceProvider tripleDesCryptoService = new TripleDESCryptoServiceProvider())
            {
                Byte[] stringToEncryptBytes = UTF8Encoding.UTF8.GetBytes(stringToEncrypt);
                tripleDesCryptoService.Key = keyArray;
                tripleDesCryptoService.Mode = CipherMode.ECB;
                tripleDesCryptoService.Padding = PaddingMode.PKCS7;

                ICryptoTransform cryptoTransformer = tripleDesCryptoService.CreateEncryptor();
                Byte[] encryptBytes = cryptoTransformer.TransformFinalBlock(stringToEncryptBytes, 0, stringToEncryptBytes.Length);

                return Convert.ToBase64String(encryptBytes);
            }
        }

        public String Decrypt(String stringToDecrypt)
        {
            using (TripleDESCryptoServiceProvider tripleDesCryptoService = new TripleDESCryptoServiceProvider())
            {
                Byte[] stringToDecryptBytes = Convert.FromBase64String(stringToDecrypt);
                tripleDesCryptoService.Key = keyArray;
                tripleDesCryptoService.Mode = CipherMode.ECB;
                tripleDesCryptoService.Padding = PaddingMode.PKCS7;

                ICryptoTransform cryptoTransformer = tripleDesCryptoService.CreateDecryptor();
                Byte[] decryptBytes = cryptoTransformer.TransformFinalBlock(stringToDecryptBytes, 0, stringToDecryptBytes.Length);

                return UTF8Encoding.UTF8.GetString(decryptBytes);
            }
        }
    }
}