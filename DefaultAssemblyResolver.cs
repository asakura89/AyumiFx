using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reflx {
    public static class DefaultAssemblyResolver {
        public static Assembly Resolve(Object sender, ResolveEventArgs args) {
            try {
                if (args.Name.Contains(".resources"))
                    return null;

                Assembly asm = AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a => a.FullName == args.Name);

                if (asm != null)
                    return asm;
            }
            catch {
                return null;
            }

            String[] parts = args.Name.Split(',');
            String file = $"{Path.GetDirectoryName(args.RequestingAssembly.Location)}\\{parts[0].Trim()}.dll";

            return Assembly.Load(file);
        }
    }
}
