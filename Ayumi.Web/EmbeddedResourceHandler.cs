using System;
using System.IO;
using System.Reflection;

namespace WebApp {
    public static class EmbeddedResourceHandler {
        public static String GetTextFile(String filename) => GetTextFile(Assembly.GetExecutingAssembly(), filename);

        public static String GetTextFile(Assembly asm, String filename) {
            using (Stream stream = asm.GetManifestResourceStream(filename)) {
                if (stream == null)
                    return null;

                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}