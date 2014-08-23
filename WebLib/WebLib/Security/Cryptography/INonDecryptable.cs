using System;

namespace WebLib.Security.Cryptography
{
    public interface INonDecryptable
    {
        String GetHash(String stringToHash);
        Boolean IsHashVerified(String stringToHash, String againtsHashString);
    }
}