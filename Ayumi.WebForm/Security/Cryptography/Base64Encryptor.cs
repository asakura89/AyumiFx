using System;
using System.Text;

namespace WebLib.Security.Cryptography
{
    public class Base64Encryptor : IDecryptable
    {
        /*public String GetHash(String stringToHash)
        {
            Byte[] hashBytes = Encoding.UTF8.GetBytes(stringToHash);
            return Convert.ToBase64String()
        }

        public Byte[] GetHashBytes(String stringToHash)
        {
            Byte[] hashBytes = Encoding.UTF8.GetBytes(GetHash(stringToHash));
            return hashBytes;
        }

        public Boolean IsHashVerified(String stringToHash, String againtsHashString)
        {
            String hashStringToVerified = GetHash(stringToHash);
            Int32 compareResult = StringComparer.OrdinalIgnoreCase.Compare(hashStringToVerified, againtsHashString);

            return compareResult == 0;
        }*/

        public String Encrypt(string stringToEncrypt)
        {
            Byte[] encodedBytes = Encoding.UTF8.GetBytes(stringToEncrypt);
            return Convert.ToBase64String(encodedBytes);
        }

        public String Decrypt(string stringToDecrypt)
        {
            Byte[] hashBytes = Convert.FromBase64String(stringToDecrypt);
            return Encoding.UTF8.GetString(hashBytes);
        }
    }
}