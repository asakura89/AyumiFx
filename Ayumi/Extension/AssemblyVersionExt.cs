using System;
using System.Linq;
using System.Reflection;

namespace Ayumi.Extension {
    public static class AssemblyVersionExt {
        static AssemblyVersionAttribute GetVersion(this Assembly asm) => asm
            .GetCustomAttributes(typeof(AssemblyVersionAttribute), false)
            .Cast<AssemblyVersionAttribute>()
            .SingleOrDefault();

        static AssemblyFileVersionAttribute GetFileVersion(this Assembly asm) => asm
            .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
            .Cast<AssemblyFileVersionAttribute>()
            .SingleOrDefault();

        public static String GetAppVersion(this Assembly asm) {
            AssemblyVersionAttribute asmVersion = asm.GetVersion();
            if (asmVersion != null)
                return asmVersion.Version;

            AssemblyFileVersionAttribute asmFileVersion = asm.GetFileVersion();
            if (asmFileVersion != null)
                return asmFileVersion.Version;

            return "1.0.0.0";
        }
    }
}
