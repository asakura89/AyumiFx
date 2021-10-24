using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace WebApp.Config {
    public static class MainConfig {
        public static String ConnectionString => ConfigurationManager
            .ConnectionStrings[Section.ConnectionStringName]
            .ConnectionString;

        public static Section Section => ConfigurationManager.GetSection("WebApp") as Section ?? new Section();

        static AssemblyVersionAttribute AsmVersion => Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes(typeof(AssemblyVersionAttribute), false)
            .Cast<AssemblyVersionAttribute>()
            .SingleOrDefault();

        static AssemblyFileVersionAttribute AsmFileVersion => Assembly
            .GetExecutingAssembly()
            .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
            .Cast<AssemblyFileVersionAttribute>()
            .SingleOrDefault();

        public static String AppVersion =>
            AsmVersion != null ? AsmVersion.Version : AsmFileVersion != null ? AsmFileVersion.Version : "1.0.0.0";

        public static String TenantEnx => Security.Encrypt(Section.Tenant,
            Security.EncodeBase64Url(Section.AppFullName), Security.EncodeBase64Url(Section.AppFullName));

        public static String SaltEnx => Security.Encrypt(Section.SecuritySalt,
            Security.EncodeBase64Url(Section.AppFullName + TenantEnx),
            Security.EncodeBase64Url(Section.AppFullName + TenantEnx));

        public static String AdminGroupNameDnx => Security.Decrypt(Section.AdminGroupName,
            Security.EncodeBase64Url(SaltEnx), Security.EncodeBase64Url(SaltEnx));

        public static String NewUserInitialPasswordDnx => Security.Decrypt(Section.NewUserInitialPassword,
            Security.EncodeBase64Url(SaltEnx), Security.EncodeBase64Url(SaltEnx));
    }
}