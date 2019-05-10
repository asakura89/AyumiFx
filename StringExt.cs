using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Varya {
    public static class StringExt {
        public static String ReplaceWith(this String string2Replace, IDictionary<String, String> replacements) {
            if (replacements != null && replacements.Any()) {
                return Regex.Replace(string2Replace, "\\$\\{(?<key>\\w*)\\}", match => {
                    if (match.Success) {
                        String key = match.Groups["key"].Value;
                        if (replacements.ContainsKey(key))
                            return replacements[key];

                        return key;
                    }

                    return String.Empty;
                });
            }

            return string2Replace;
        }
    }
}
