using System;
using System.IO;
using System.Reflection;

namespace Ayumi.Extension {
    public static class EmbeddedResourceExt {
        public static String GetTextFile(String filename) => GetTextFile(Assembly.GetExecutingAssembly(), filename);

        public static String GetTextFile(this Assembly asm, String filename) {
            using (Stream stream = asm.GetManifestResourceStream(filename)) {
                if (stream == null)
                    return null;

                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}