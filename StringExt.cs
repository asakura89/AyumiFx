using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Varya {
    public static class StringExt {
        public static String ReplaceWith(this String string2Replace, IDictionary<String, String> replacements) {
            if (replacements != null && replacements.Any()) {
                return Regex.Replace(string2Replace, "\\$\\{(?<key>\\w*)\\}", match => {
                    String key = match.Groups["key"].Value;
                    if (replacements.ContainsKey(key))
                        return replacements[key];

                    return match.Value;
                });
            }

            return string2Replace;
        }

        public static String ReplaceWith(this String string2Replace, IDictionary<String, IEnumerable<String>> replacements) {
            if (replacements != null && replacements.Any()) {
                return Regex.Replace(string2Replace, "\\$\\{(?<key>\\w*):(?<index>\\d)\\}", match => {
                    String key = match.Groups["key"].Value;
                    Int32 index = Convert.ToInt32(match.Groups["index"].Value);
                    if (replacements.ContainsKey(key)) {
                        String[] array = replacements[key].ToArray();
                        return array[index];
                    }

                    return match.Value;
                });
            }

            return string2Replace;
        }
    }
}
