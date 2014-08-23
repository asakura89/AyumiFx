using System;
using System.Security.Cryptography;
using System.Text;

namespace WebLib.Security.Cryptography
{
    public class SimpleOneWayEncryptor : INonDecryptable
    {
        private readonly HashAlgorithm algorithm;

        public SimpleOneWayEncryptor(HashAlgorithm algorithm)
        {
            if (algorithm == null)
                throw new ArgumentNullException("algorithm");

            this.algorithm = algorithm;
        }

        public String GetHash(String stringToHash)
        {
            Byte[] hashBytes = algorithm.ComputeHash(Encoding.Default.GetBytes(stringToHash));
            StringBuilder hashStringBuilder = new StringBuilder();

            foreach (Byte hashByte in hashBytes)
                hashStringBuilder.Append(hashByte.ToString("x2"));

            return hashStringBuilder.ToString();
        }

        public Boolean IsHashVerified(String stringToHash, String againtsHashString)
        {
            String hashStringToVerified = GetHash(stringToHash);
            Int32 compareResult = StringComparer.OrdinalIgnoreCase.Compare(hashStringToVerified, againtsHashString);

            return compareResult == 0;
        } 
    }
}