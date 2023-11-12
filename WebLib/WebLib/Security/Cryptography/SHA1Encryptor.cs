using System;
using System.Security.Cryptography;

namespace WebLib.Security.Cryptography
{
    public class SHA1Encryptor : INonDecryptable
    {
        private readonly SimpleOneWayEncryptor encryptor;

        public SHA1Encryptor()
        {
            using (SHA1 sha1Algorithm = SHA1.Create())
                encryptor = new SimpleOneWayEncryptor(sha1Algorithm);
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