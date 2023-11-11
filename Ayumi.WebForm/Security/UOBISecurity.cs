using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebLib.Security
{
    public class UOBISecurity
    {
        public class Key
        {
            public String ClearKey { get; set; }
            public List<String> EncryptedKey { get; set; }

            public Key()
            {
                EncryptedKey = new List<String>();
            }

            public void EncryptKey(Int32 splittingCount)
            {
                String str = new Encryptor().Encrypt(ClearKey, (HashAlgorithm)SHA256.Create());
                Int32 length = str.Length / splittingCount;
                Int32 startIndex = 0;
                while (startIndex < str.Length)
                {
                    if (startIndex + length * 2 > str.Length)
                    {
                        EncryptedKey.Add(str.Substring(startIndex, str.Length - startIndex));
                        break;
                    }
                    else
                    {
                        EncryptedKey.Add(str.Substring(startIndex, length));
                        startIndex += length;
                    }
                }
            }

            public void EncryptKey()
            {
                EncryptKey(0);
            }
        }
    }

    public class Encryptor
    {
        private TripleDESCryptoServiceProvider m_des;
        private byte[] m_iv;
        private UTF8Encoding m_utf8;
        private string ASCII;

        public Encryptor()
        {
            this.m_des = new TripleDESCryptoServiceProvider();
            this.m_iv = new byte[8]
      {
        (byte) 8,
        (byte) 7,
        (byte) 6,
        (byte) 5,
        (byte) 4,
        (byte) 3,
        (byte) 2,
        (byte) 1
      };
            this.m_utf8 = new UTF8Encoding();
            this.ASCII = ".?q`w~e!r@t#y$ui^op*a(s)d_f+ gh{jk-l]z:x[c}vbnmQWER|T0Y1U2I3O4P5A6S7D8F\\9GHJ\"KL/ZX=C<V'B%>N&M;,";
        }

        public string Encrypt(string value, string masterKey)
        {
            UOBIEncrypt uobiEncrypt = new UOBIEncrypt();
            byte[] bytes = this.m_utf8.GetBytes(uobiEncrypt.MaskValue(value, this.ASCII));
            return uobiEncrypt.Encrypt(bytes, this.m_des.CreateEncryptor(Encoding.ASCII.GetBytes(masterKey.Substring(0, 24)), this.m_iv));
        }

        public string Encrypt(string value, string key1, string key2)
        {
            return this.Encrypt(value, this.Encrypt(key1, (HashAlgorithm)SHA1.Create()) + this.Encrypt(key2, (HashAlgorithm)RIPEMD160.Create()));
        }

        public string Encrypt(string value, HashAlgorithm algorithm)
        {
            return new UOBIEncrypt().Encrypt(value, algorithm);
        }

        public string Decrypt(string value, string masterKey)
        {
            UOBIDecrypt uobiDecrypt = new UOBIDecrypt();
            string @string = this.m_utf8.GetString(uobiDecrypt.Decrypt(value, this.m_des.CreateDecryptor(Encoding.ASCII.GetBytes(masterKey.Substring(0, 24)), this.m_iv)));
            return uobiDecrypt.UnMaskValue(@string, this.ASCII);
        }

        public string Decrypt(string value, string key1, string key2)
        {
            return this.Decrypt(value, this.Encrypt(key1, (HashAlgorithm)SHA1.Create()) + this.Encrypt(key2, (HashAlgorithm)RIPEMD160.Create()));
        }
    }

    internal class UOBIEncrypt
    {

        public string Encrypt(byte[] input, ICryptoTransform encryptor)
        {
            return Convert.ToBase64String(this.Transform(input, encryptor));
        }

        public string Encrypt(string value, HashAlgorithm algorithm)
        {
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
                stringBuilder.Append(num.ToString("X2"));
            return ((object)stringBuilder).ToString();
        }

        public string MaskValue(string _Value, string ASCII)
        {
            if (_Value == null)
                _Value = "";
            int startIndex1 = new Random().Next(0, ASCII.Length - 1);
            string str = ASCII.Substring(startIndex1, 1);
            try
            {
                if (_Value.Length > 0)
                {
                    for (int startIndex2 = 0; startIndex2 <= _Value.Length - 1; ++startIndex2)
                    {
                        int startIndex3 = ASCII.IndexOf(_Value.Substring(startIndex2, 1)) + startIndex1;
                        if (startIndex3 >= 0)
                        {
                            if (startIndex3 >= ASCII.Length)
                                startIndex3 -= ASCII.Length;
                            str = str + ASCII.Substring(startIndex3, 1);
                        }
                        else
                            str = str + _Value.Substring(startIndex2, 1);
                    }
                }
                else
                    str = _Value;
            }
            catch (Exception ex)
            {
            }
            return str;
        }

        private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, CryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();
            memoryStream.Position = 0L;
            byte[] buffer = new byte[Convert.ToInt32(memoryStream.Length - 1L) + 1];
            memoryStream.Read(buffer, 0, Convert.ToInt32(buffer.Length));
            memoryStream.Close();
            cryptoStream.Close();
            return buffer;
        }
    }

    internal class UOBIDecrypt
    {

        public byte[] Decrypt(string value, ICryptoTransform decryptor)
        {
            return this.Transform(Convert.FromBase64String(value), decryptor);
        }

        public string UnMaskValue(string _Value, string ASCII)
        {
            string str = "";
            try
            {
                if (_Value.Length > 0)
                {
                    int num1 = ASCII.IndexOf(_Value.Substring(0, 1));
                    for (int startIndex1 = 1; startIndex1 <= _Value.Length - 1; ++startIndex1)
                    {
                        int num2 = ASCII.IndexOf(_Value.Substring(startIndex1, 1));
                        if (num2 >= 0)
                        {
                            int startIndex2 = num2 - num1;
                            if (startIndex2 < 0)
                                startIndex2 = ASCII.Length + startIndex2;
                            str = str + ASCII.Substring(startIndex2, 1);
                        }
                        else
                            str = str + _Value.Substring(startIndex1, 1);
                    }
                }
                else
                    str = _Value;
            }
            catch (Exception ex)
            {
            }
            return str;
        }

        private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, CryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();
            memoryStream.Position = 0L;
            byte[] buffer = new byte[Convert.ToInt32(memoryStream.Length - 1L) + 1];
            memoryStream.Read(buffer, 0, Convert.ToInt32(buffer.Length));
            memoryStream.Close();
            cryptoStream.Close();
            return buffer;
        }
    }
}