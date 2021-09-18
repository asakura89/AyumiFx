using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Nvy;

namespace Reflx {
    public static class ReflectionExt {
        public static TAttribute GetDecorator<TAttribute>(this Type type) =>
            type == null ? default(TAttribute) : type
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();

        public static TAttribute GetDecorator<TAttribute>(this Assembly assembly) =>
            assembly == null ? default(TAttribute) : assembly
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();

        public static TAttribute GetDecorator<TAttribute>(this FieldInfo field) =>
            field == null ? default(TAttribute) : field
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();

        public static TAttribute GetDecorator<TAttribute>(this PropertyInfo property) =>
            property == null ? default(TAttribute) : property
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();

        public static TAttribute GetDecorator<TAttribute>(this MemberInfo member) =>
            member == null ? default(TAttribute) : member
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();

        public static TAttribute GetDecorator<TAttribute>(this ParameterInfo parameter) =>
            parameter == null ? default(TAttribute) : parameter
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .SingleOrDefault();

        public static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> assemblies) =>
            assemblies?.SelectMany(asm => asm.GetTypes());

        public static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> assemblies, Func<Type, Boolean> predicate) =>
            assemblies.GetTypes().Where(predicate);

        public static IEnumerable<TAttribute> GetDecorators<TAttribute>(this IEnumerable<Type> types) =>
            types?
                .SelectMany(type => type.GetCustomAttributes(false))
                .Cast<TAttribute>();

        public static IEnumerable<TAttribute> GetDecorators<TAttribute>(this IEnumerable<Assembly> assemblies) =>
            assemblies
                .GetTypes()
                .Select(GetDecorator<TAttribute>);

        public static Boolean DecoratedBy<TAttribute>(this Type type) =>
            type
                .GetCustomAttributes(typeof(TAttribute), false)
                .Any();

        public static IEnumerable<Type> GetTypesDecoratedBy<TAttribute>(this IEnumerable<Type> types) =>
            types?.Where(DecoratedBy<TAttribute>);

        public static IEnumerable<Type> GetTypesDecoratedBy<TAttribute>(this IEnumerable<Assembly> assemblies) =>
            assemblies
                .GetTypes()
                .GetTypesDecoratedBy<TAttribute>();

        public static Type GetSingleTypeDecoratedBy<TAttribute>(this IEnumerable<Type> types, Func<TAttribute, Boolean> predicate) =>
            types?.FirstOrDefault(type => predicate(type.GetDecorator<TAttribute>()));

        public static Type GetSingleTypeDecoratedBy<TAttribute>(this IEnumerable<Assembly> assemblies, Func<TAttribute, Boolean> predicate) =>
            assemblies
                .GetTypes()
                .GetSingleTypeDecoratedBy(predicate);

        public static Boolean InheritedBy<TAncestor>(this Type type) =>
            type != null && typeof(TAncestor).IsAssignableFrom(type);

        public static IEnumerable<Type> GetTypesInheritedBy<TAncestor>(this IEnumerable<Type> types) =>
            types?.Where(InheritedBy<TAncestor>);

        public static IEnumerable<Type> GetTypesInheritedBy<TAncestor>(this IEnumerable<Assembly> assemblies) =>
            assemblies.GetTypes(InheritedBy<TAncestor>);

        public static Type GetType(this IEnumerable<Assembly> assemblies, Func<Type, Boolean> predicate) =>
            assemblies?
                .SelectMany(asm => asm.GetTypes())
                .FirstOrDefault(predicate);

        public static Type GetSingleTypeInheritedBy<TAncestor>(this IEnumerable<Type> types) =>
            types?.FirstOrDefault(InheritedBy<TAncestor>);

        public static Type GetSingleTypeInheritedBy<TAncestor>(this IEnumerable<Assembly> assemblies) =>
            assemblies.GetType(InheritedBy<TAncestor>);

        public static void LoadAssemblies(String path) =>
            LoadAssemblies(path, new[] { "*" });

        public static void LoadAssemblies(String path, IEnumerable<String> assemblyNames) {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);
            if (assemblyNames == null)
                throw new ArgumentNullException(nameof(assemblyNames));
            if (!assemblyNames.Any())
                throw new ArgumentOutOfRangeException(nameof(assemblyNames));

            if (assemblyNames.Any(ns => ns.Equals("*")))
                assemblyNames = Directory
                    .GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly)
                    .Select(ns => ns.Replace(".dll", String.Empty));

            IList<String> goodNamespaces = assemblyNames
                .Where(ns => !String.IsNullOrEmpty(ns))
                .ToList();

            if (!goodNamespaces.Any())
                throw new ArgumentOutOfRangeException(nameof(assemblyNames));

            IEnumerable<NameValueItem> nonExistents = goodNamespaces
                .Select(ns => Path.Combine(path, ns + ".dll"))
                .Select(ns => new NameValueItem(ns, File.Exists(ns).ToString()))
                .Where(asm => !Convert.ToBoolean(asm.Value));

            if (nonExistents.Any()) {
                String message = $"Below assemblies are nowhere to be found. {Environment.NewLine}{String.Join(Environment.NewLine, nonExistents.Select(item => item.Name))}";
                throw new FileNotFoundException(message);
            }

            // https://stackoverflow.com/questions/9315716/side-effects-of-calling-assembly-load-multiple-times
            foreach (String ns in goodNamespaces) {
                try {
                    String file = Path.Combine(path, ns + ".dll");
                    var asmName = AssemblyName.GetAssemblyName(file);
                    Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == asmName.FullName);
                    if (asm == null)
                        Assembly.Load(File.ReadAllBytes(file));
                }
                catch {
                    //
                }
            }
        }
    }
}
