using System.Collections.Generic;
using System.Linq;

namespace Ayumi.Extension {
    public static class TypeExt {
        public static IList<TAttribute> GetDecorators<TAttribute, T>(this T obj) where T : class => obj
            .GetType()
            .GetCustomAttributes(typeof(TAttribute), false)
            .Cast<TAttribute>()
            .ToList();

        public static IList<TAttribute> GetMemberDecorators<TAttribute, T>(this T obj) where T : class => obj
            .GetType()
            .GetProperties()
            .SelectMany(prop => prop
                .GetCustomAttributes(typeof(TAttribute), false))
            .Cast<TAttribute>()
            .ToList();
    }
}
