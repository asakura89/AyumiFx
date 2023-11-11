using System;
using System.Collections.Generic;
using System.Text;

namespace WebLib.Security.Cryptography
{
    public class WebConfigEncryptor : IDecryptable
    {
        /*public class Key
        {
            public String ClearKey { get; set; }
            public List<String> EncryptedKeyParts { get; set; }

            public Key()
            {
                EncryptedKeyParts = new List<String>();
            }
        }

        private IEnumerable<String> EncryptKey(String plainKey)
        {
            TripleDESEncryptor encryptor = new TripleDESEncryptor();
            String randomASCIIString = ".?q`w~e!r@t#y$ui^op*a(s)d_f+ gh{jk-l]z:x[c}vbnmQWER|T0Y1U2I3O4P5A6S7D8F\\9GHJ\"KL/ZX=C<V'B%>N&M;,";
            Byte[] iv = new Byte[8]{ 8, 7, 6, 5, 4, 3, 2, 1 };
            UTF8Encoding utf8Encoding = new UTF8Encoding();

            String maskedKey = MaskStringValue(plainKey);
            Byte[] keyBytes = utf8Encoding.GetBytes(maskedKey);

            String encryptedKey = "";
        }

        private String MaskStringValue(String stringValue)
        {
            
        }

        private String UnmaskStringValue(String stringValue)
        {
            
        }*/

        public String Encrypt(String stringToEncrypt)
        {
            throw new System.NotImplementedException();
        }

        public String Decrypt(String stringToDecrypt)
        {
            throw new System.NotImplementedException();
        }
    }
}