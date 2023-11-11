using System;
using System.Collections.Generic;

namespace Ayumi.Extensions
{
    public static class ExtendedString
    {
        public static IEnumerable<String> Cut(this String originalString, Int32 cutSize)
        {
            for (int index = 0, stringLength = originalString.Length; index < stringLength; index += cutSize)
            {
                Int32 startIndex = (index > stringLength) ? stringLength : index;
                Int32 currentCutSize = cutSize > (stringLength - index) ? (stringLength - index) : cutSize;
                String subtractedString = originalString.Substring(startIndex, currentCutSize);
                yield return subtractedString;
            }
        }

        public static String TrimStart(this String target, String trimString)
        {
            String result = target;
            while (result.StartsWith(trimString))
                result = result.Substring(trimString.Length);

            return result;
        }

        public static String TrimEnd(this String target, String trimString)
        {
            String result = target;
            while (result.EndsWith(trimString))
                result = result.Substring(0, result.Length - trimString.Length);

            return result;
        }
    }
}
