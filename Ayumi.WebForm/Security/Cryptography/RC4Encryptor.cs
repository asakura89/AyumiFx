using System;
using System.Text;

namespace WebLib.Security.Cryptography
{
    public class RC4Encryptor : INonDecryptable
    {
        public String GetHash(String stringToHash)
        {
            int[] sbox = new int[256];
            int[] sbox2 = new int[256];
            int i, modLen, j;
            int t, k, temp;
            string output = "";
            string key = "1234567";
            int len = key.Length;
            i = 0;
            for (i = 0; i <= 255; i++)
            {
                sbox[i] = Convert.ToByte(i);
                modLen = i % len;
                sbox2[i] = (int)Convert.ToChar(key.Substring(modLen, 1));
                sbox[2].ToString();
            }

            j = 0;
            for (i = 0; i <= 255; i++)
            {
                j = (j + sbox[i] + sbox2[i]) % 256;
                temp = sbox[i];
                sbox[i] = sbox[j];
                sbox[j] = temp;
            }

            i = 0; j = 0;
            for (int x = 1; x <= stringToHash.Length; x++)
            {
                i = (i + 1) % 256;
                j = (j + sbox[i]) % 256;
                temp = sbox[i];
                sbox[i] = sbox[j];
                sbox[j] = temp;
                t = (sbox[i] + sbox[j]) % 256;
                k = sbox[t];
                int v = (int)((int)(Convert.ToChar(stringToHash.Substring(x - 1, 1))) ^ (int)k);
                //Console.WriteLine("k.." + k + "charby..." + v + " v " + Convert.ToChar(v));
                output = output + Convert.ToChar(v);
            }
            return output;
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
        }
    }
}