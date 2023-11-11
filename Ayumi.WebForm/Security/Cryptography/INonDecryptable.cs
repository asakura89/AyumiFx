using System;

namespace WebLib.Security.Cryptography
{
    public interface INonDecryptable
    {
        String GetHash(String stringToHash);
        Byte[] GetHashBytes(String stringToHash);
        Boolean IsHashVerified(String stringToHash, String againtsHashString);
    }
}