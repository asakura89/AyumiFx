using System;
using System.Collections.Generic;
using System.Linq;

namespace Ayumi.Extension {
    public static class ObjectExt {
        public static String AsDelimitedString<T>(this T t, String itemDelimiter, params Func<T, String>[] tSelector) where T : class =>
            String.Join(itemDelimiter, tSelector.Select(selector => selector(t)));

        public static String AsDelimitedString<T>(this IEnumerable<T> tList, String itemDelimiter, String listDelimiter, params Func<T, String>[] tSelector) where T : class {
            IEnumerable<String> delimitedStringList = tList.Select(t => t.AsDelimitedString(itemDelimiter, tSelector));
            String delimitedString = String.Join(listDelimiter, delimitedStringList);

            return delimitedString;
        }
    }
}
