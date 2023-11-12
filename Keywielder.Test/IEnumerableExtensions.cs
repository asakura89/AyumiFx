using System;
using System.Collections;

namespace Keywielder.Test {
    public static class IEnumerableExtensions {
        public static void CheckDuplicate(this IList suspectedList) {
            if (suspectedList.ContainsDuplicate())
                throw new Exception("Duplicate!!!");
        }

        public static Boolean ContainsDuplicate(this IList suspectedList) =>
            suspectedList.GetDuplicate().Count() > 0;

        public static Int32 Count(this IEnumerable source) {
            Int32 res = 0;
            foreach (Object item in source)
                res++;

            return res;
        }

        public static IEnumerable GetDuplicate(this IList suspectedList) {
            for (Int32 i = 0; i < suspectedList.Count -1; i++)
                for (Int32 j = i + 1; j < suspectedList.Count; j++)
                    if (suspectedList[i].Equals(suspectedList[j]))
                        yield return suspectedList[i];
        }
    }
}