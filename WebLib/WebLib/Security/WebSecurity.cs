using System;
using System.Security.Cryptography;
using System.Text;

namespace WebLib.Security
{
    public class WebSecurity
    {
        #region MD5
        public string getMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();            
        }
        public bool verifyMd5Hash(string input, string hash)
        {
            string hashOfInput = getMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, getMd5Hash(hash)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region SHA384
        public string getSHA384Hash(string input)
        {
            SHA384 SHA384Hasher = SHA384.Create();
            byte[] data = SHA384Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public bool verifySHA384Hash(string input, string hash)
        {
            string hashOfInput = getSHA384Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, getSHA384Hash(hash)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region RC4
        public string getRC4Hash(string input)
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
            for (int x = 1; x <= input.Length; x++)
            {
                i = (i + 1) % 256;
                j = (j + sbox[i]) % 256;
                temp = sbox[i];
                sbox[i] = sbox[j];
                sbox[j] = temp;
                t = (sbox[i] + sbox[j]) % 256;
                k = sbox[t];
                int v = (int)((int)(Convert.ToChar(input.Substring(x - 1, 1))) ^ (int)k);
                //Console.WriteLine("k.." + k + "charby..." + v + " v " + Convert.ToChar(v));
                output = output + Convert.ToChar(v);
            }
            return output;

        }
        public bool verifyRC4Hash(string input, string hash)
        {
            string hashOfInput = getRC4Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, getRC4Hash(hash)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Base64
        public string getBase64Hash(string input)
        {

            byte[] data = Encoding.Default.GetBytes(input);
            return Convert.ToBase64String(data);
        }
        public bool verifyBase64Hash(string input, string hash)
        {
            string hashOfInput = getBase64Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, getBase64Hash(hash)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region TripleDES
        /// <summary>
        /// TripleDes Encryption with 16Bit, so key must 16 characters
        /// </summary>
        /// <param name="toEncrypt">is word that you want to encrypt</param>
        /// <param name="useHashing">for condition you want use hashing or not</param>
        /// <param name="key">for key to encrypt</param>
        /// <returns>string</returns>
        public string EncryptTripleDES(string toEncrypt, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                string key = "!Int3rnalSupp0rt";

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// TripleDes Decryption with 16Bit, so key must 16 characters
        /// </summary>
        /// <param name="cipherString">is word that you want to decrypt</param>
        /// <param name="useHashing">for condition you want use hashing or not</param>
        /// <param name="key">for key to decrypt, you must make sure the key is same with encrypt key</param>
        /// <returns>string</returns>
        public string DecryptTripleDes(string cipherString, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);


                string key = "!Int3rnalSupp0rt";

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                return null;

            }
        }
        #endregion
    }
}
