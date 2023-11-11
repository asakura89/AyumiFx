using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ayumi.Extension;

namespace Ayumi.Data.Query {
    public static class EqxLoader {
        public static String Load<T>(T caller, String filename) => Load($"{caller.GetType().Name}/{filename}");

        public static String Load(String filename) {
            String rootDir = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath);
            String combined = null;
            if (filename.Contains("/")) {
                String[] splitted = filename.Split('/');
                combined = splitted.Aggregate("Eqx", Path.Combine);
            }

            if (String.IsNullOrEmpty(rootDir) || !Directory.Exists(rootDir))
                throw new InvalidOperationException("Invalid eqx file.");

            String eqxPath = Path.Combine(rootDir, combined ?? filename) + ".eqx";
            if (!File.Exists(eqxPath))
                throw new InvalidOperationException("Invalid eqx file.");

            using (var stream = new FileStream(eqxPath, FileMode.Open))
                using (var streamR = new StreamReader(stream, Encoding.UTF8))
                    return streamR.ReadToEnd();
        }

        public static String LoadEmbedded<T>(T caller, String filename) {
            Assembly asm = caller.GetType().Assembly;
            return asm.GetTextFile($"{asm.GetName().Name}.Eqx.{caller.GetType().Name}.{filename}.eqx");
        }
    }
}