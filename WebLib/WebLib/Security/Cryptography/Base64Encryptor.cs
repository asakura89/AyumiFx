using System;
using System.Text;

namespace WebLib.Security.Cryptography
{
    public class Base64Encryptor : INonDecryptable
    {
        public String GetHash(String stringToHash)
        {
            Byte[] hashBytes = Encoding.Default.GetBytes(stringToHash);
            return Convert.ToBase64String(hashBytes);
        }

        public Boolean IsHashVerified(String stringToHash, String againtsHashString)
        {
            String hashStringToVerified = GetHash(stringToHash);
            Int32 compareResult = StringComparer.OrdinalIgnoreCase.Compare(hashStringToVerified, againtsHashString);

            return compareResult == 0;
        }
    }
}