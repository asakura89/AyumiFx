using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Varya {
    public static class StringExt {
        public static String ReplaceWith(this String string2Replace, IDictionary<String, String> replacements) {
            if (replacements != null && replacements.Any()) {
                return Regex.Replace(string2Replace, "(\\$\\w*)", match => {
                    if (match.Success) {
                        if (replacements.ContainsKey(match.Value))
                            return replacements[match.Value];

                        return match.Value;
                    }

                    return String.Empty;
                });
            }

            return string2Replace;
        }
    }
}
