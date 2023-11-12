using System;
using System.Security.Cryptography;

namespace WebLib.Security.Cryptography
{
    public class MD5Encryptor : INonDecryptable
    {
        private readonly SimpleOneWayEncryptor encryptor;

        public MD5Encryptor()
        {
            using (MD5 md5Algorithm = MD5.Create())
                encryptor = new SimpleOneWayEncryptor(md5Algorithm);
        }

        public String GetHash(String stringToHash)
        {
            return encryptor.GetHash(stringToHash);
        }

        public Byte[] GetHashBytes(String stringToHash)
        {
            return encryptor.GetHashBytes(stringToHash);
        }

        public Boolean IsHashVerified(String stringToHash, String againtsHashString)
        {
            return encryptor.IsHashVerified(stringToHash, againtsHashString);
        }
    }
}