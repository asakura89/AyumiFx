using System;
using System.Security.Cryptography;

namespace WebLib.Security.Cryptography
{
    public class SHA384Encryptor : INonDecryptable
    {
        private readonly SimpleOneWayEncryptor encryptor;

        public SHA384Encryptor()
        {
            using (SHA384 sha384Algorithm = SHA384.Create())
                encryptor = new SimpleOneWayEncryptor(sha384Algorithm);
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