using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebApp {
    public static class Eqx {
        public static String Load<T>(T caller, String filename) => caller.LoadEqx(filename);

        public static String LoadEqx<T>(this T caller, String filename) => Load($"{caller.GetType().Name}/{filename}");

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

        public static String LoadEmbedded<T>(T caller, String filename) => caller.LoadEmbeddedEqx(filename);

        public static String LoadEmbeddedEqx<T>(this T caller, String filename) {
            Assembly asm = caller.GetType().Assembly;
            return EmbeddedResourceHandler
                .GetTextFile(asm,
                    $"{asm.GetName().Name}.Eqx.{caller.GetType().Name}.{filename}.eqx");
        }
    }
}