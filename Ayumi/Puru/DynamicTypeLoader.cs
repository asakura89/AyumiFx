using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Nvy;

namespace Puru {
    public class DynamicTypeLoader {
        readonly String loadFromPath;
        readonly IEnumerable<String> namespaces;

        public DynamicTypeLoader(String loadFromPath) : this(loadFromPath, new[] { "*" }) { }

        public DynamicTypeLoader(String loadFromPath, IEnumerable<String> assemblyNames) {
            if (String.IsNullOrEmpty(loadFromPath))
                throw new ArgumentNullException(nameof(loadFromPath));
            if (!Directory.Exists(loadFromPath))
                throw new DirectoryNotFoundException(loadFromPath);
            if (assemblyNames == null)
                throw new ArgumentNullException(nameof(assemblyNames));
            if (!assemblyNames.Any())
                throw new ArgumentOutOfRangeException(nameof(assemblyNames));

            if (assemblyNames.Any(ns => ns.Equals("*")))
                assemblyNames = Directory
                    .EnumerateFiles(loadFromPath, "*.dll", SearchOption.TopDirectoryOnly)
                    .Select(ns => ns.Replace(".dll", String.Empty));

            IList<String> goodNamespaces = assemblyNames
                .Where(ns => !String.IsNullOrEmpty(ns))
                .ToList();

            if (!goodNamespaces.Any())
                throw new ArgumentOutOfRangeException(nameof(assemblyNames));

            IEnumerable<NameValueItem> nonExistents = goodNamespaces
                .Select(ns => Path.Combine(loadFromPath, ns + ".dll"))
                .Select(ns => new NameValueItem(ns, File.Exists(ns).ToString()))
                .Where(asm => !Convert.ToBoolean(asm.Value));

            if (nonExistents.Any()) {
                String message = $"Below assemblies are nowhere to be found. {Environment.NewLine}{String.Join(Environment.NewLine, nonExistents.Select(item => item.Name))}";
                throw new FileNotFoundException(message);
            }

            namespaces = goodNamespaces;
            this.loadFromPath = loadFromPath;
        }

        public DynamicTypeLoader(IEnumerable<String> assemblyNames) : this(AppDomain.CurrentDomain.BaseDirectory, assemblyNames) { }

        public DynamicTypeLoader() : this(AppDomain.CurrentDomain.BaseDirectory) { }

        IList<Assembly> LoadAssemblies() {
            // NOTE: Thank God it'll load once
            // https://stackoverflow.com/questions/9315716/side-effects-of-calling-assembly-load-multiple-times
            IList<Assembly> assemblies = namespaces
                .Select(ns => {
                    try {
                        return Assembly.Load(
                            File.ReadAllBytes(
                                Path.Combine(loadFromPath, ns + ".dll")));
                    }
                    catch {
                        return null;
                    }
                })
                .Where(asm => asm != null)
                .ToList();

            //assemblies = assemblies
            //    .Concat(
            //        assemblies
            //            .SelectMany(asm => asm.GetReferencedAssemblies())
            //            .Select(asmName => {
            //                try {
            //                    return Assembly.Load(
            //                        File.ReadAllBytes(
            //                            Path.Combine(loadFromPath, asmName.Name + ".dll")));
            //                }
            //                catch {
            //                    return null;
            //                }
            //            })
            //            .Where(asm => asm != null)
            //    )
            //    .GroupBy(asm => asm.FullName.ToLowerInvariant())
            //    .Select(grp => grp.OrderBy(asm => GetVersionInteger(asm)).Last())
            //    .ToList();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            return assemblies.Any() ? assemblies : null;
        }

        Assembly CurrentDomain_AssemblyResolve(Object sender, ResolveEventArgs args) {
            try {
                if (args.Name.Contains(".resources"))
                    return null;

                Assembly asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
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

        //static Int32 GetVersionInteger(Assembly asm) => asm
        //    .GetAssemblyVersion()
        //    .Split(new[] { '.', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
        //    .Select(part => part.All(c => Char.IsDigit(c)) ? Convert.ToInt32(part) : 0)
        //    .Sum();

        public static IEnumerable<Type> GetTypesDecoratedBy<TAttribute>(IEnumerable<Assembly> assemblies) =>
            assemblies?
                .SelectMany(asm => asm.GetTypes())
                .Where(type => type
                    .GetCustomAttributes(typeof(TAttribute), false)
                    .Any());

        /*

        // I don't understand why this can't work for AttendanceDataFormatAttribute but flawless for ExcelReportCollectionAttribute
        return assemblies
            .SelectMany(asm => asm.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(TAttribute), false).Any());

        // Okay, change to another way
        IEnumerable<Type> allTypes = assemblies.SelectMany(asm => asm.GetTypes());
        IEnumerable<Type> filteredTypes = allTypes
            .Where(type => type
                .GetCustomAttributes(false)
                .Cast<Attribute>()
                .Select(attr => ((Type) attr.TypeId).FullName.ToLowerInvariant())
                .Contains(typeof(TAttribute).FullName.ToLowerInvariant()));

        return filteredTypes;

        // It works for ExcelReportCollectionAttribute but still failed for AttendanceDataFormatAttribute
        // It produce error like so
        Error! : [A]Enta.Module.Attendance.Standard.AttendanceDataFormatAttribute cannot be cast to
        [B]Enta.Module.Attendance.Standard.AttendanceDataFormatAttribute. Type A originates from 'Enta.Module.Attendance.Standard,
        Version=5.0.2017.426, Culture=neutral, PublicKeyToken=null' in the context 'LoadNeither' in a byte array. Type B originates from
        'Enta.Module.Attendance.Standard, Version=5.0.2017.426, Culture=neutral, PublicKeyToken=null' in the context 'Default'
        at location 'C:\Users\dita\AppData\Local\Temp\Temporary ASP.NET Files\vs\84f83024\14a9db08\assembly\dl3\53fdd7c2\76bcd262_495ad301\Enta.Module.Attendance.Standard.dll'.

        // I think I understand the problem. It basically said, I first load AttendanceDataFormatAttribute from Enta.Module.Attendance.Standard namespace.
        // It loads into Default context. Then I dynamically load the Enta.Module.Attendance.Standard dll, and it loads into LoadNeither context.
        // Because it both has AttendanceDataFormatAttribute, and I refer to AttendanceDataFormatAttribute in Default context, it will not match
        // the AttendanceDataFormatAttribute on LoadNeither context. That's why! It explains why this code `type.GetCustomAttributes(typeof(TAttribute)` will always fail.
        // Because it both loads in different context. So, they have to be in the different assemblies.
        // Thanks to `.Cast<TAttribute>()` from GetDecorators. ＼(＾▽＾)／

        */

        public IEnumerable<Type> GetTypesDecoratedBy<TAttribute>() =>
            GetTypesDecoratedBy<TAttribute>(LoadAssemblies());

        public static IList<TAttribute> GetDecorators<TAttribute>(IEnumerable<Assembly> assemblies) =>
            GetDecorators<TAttribute>(GetTypesDecoratedBy<TAttribute>(assemblies));

        public IList<TAttribute> GetDecorators<TAttribute>() =>
            GetDecorators<TAttribute>(GetTypesDecoratedBy<TAttribute>());

        public static IList<TAttribute> GetDecorators<TAttribute>(IEnumerable<Type> types) =>
            types?
                .SelectMany(type => type.GetCustomAttributes(false))
                .Cast<TAttribute>()
                .ToList();

        public static Type GetSingleTypeDecoratedBy<TAttribute>(IEnumerable<Assembly> assemblies, Func<TAttribute, Boolean> predicate) =>
            GetSingleTypeDecoratedBy(GetTypesDecoratedBy<TAttribute>(assemblies), predicate);

        public Type GetSingleTypeDecoratedBy<TAttribute>(Func<TAttribute, Boolean> predicate) =>
            GetSingleTypeDecoratedBy<TAttribute>(LoadAssemblies(), predicate);

        public static Type GetSingleTypeDecoratedBy<TAttribute>(IEnumerable<Type> types, Func<TAttribute, Boolean> predicate) =>
            types?
                .ToList()
                .SingleOrDefault(type =>
                    predicate(type
                        .GetCustomAttributes(typeof(TAttribute), false)
                        .Cast<TAttribute>()
                        .Single()
                    )
                );

        public static IEnumerable<Type> GetTypesInheritedBy<TAncestor>(IEnumerable<Assembly> assemblies) =>
            assemblies?
                .SelectMany(asm => asm.GetTypes())
                .Where(type => typeof(TAncestor)
                    .IsAssignableFrom(type));

        public IEnumerable<Type> GetTypesInheritedBy<TAncestor>() =>
            GetTypesInheritedBy<TAncestor>(LoadAssemblies());
    }
}