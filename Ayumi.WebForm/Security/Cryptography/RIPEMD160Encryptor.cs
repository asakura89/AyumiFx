using System;
using System.Security.Cryptography;

namespace WebLib.Security.Cryptography
{
    public class RIPEMD160Encryptor : INonDecryptable
    {
        private readonly SimpleOneWayEncryptor encryptor;

        public RIPEMD160Encryptor()
        {
            using (RIPEMD160 ripemd160Algorithm = RIPEMD160.Create())
                encryptor = new SimpleOneWayEncryptor(ripemd160Algorithm);
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