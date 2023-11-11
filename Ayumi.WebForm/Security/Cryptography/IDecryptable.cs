using System;

namespace WebLib.Security.Cryptography
{
    public interface IDecryptable
    {
        String Encrypt(String stringToEncrypt);
        String Decrypt(String stringToDecrypt);
    }
}