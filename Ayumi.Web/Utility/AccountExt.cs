using System;
using WebApp.Config;

namespace WebApp.Utility {
    public static class AccountExt {
        public static Func<String, String> PasswordEnx = plain =>
            Security.Encrypt(plain,
                Security.EncodeBase64Url(MainConfig.SaltEnx),
                Security.EncodeBase64Url(MainConfig.SaltEnx));
    }
}