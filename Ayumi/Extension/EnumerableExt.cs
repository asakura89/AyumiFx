using System;
using System.Collections;
using System.Collections.Generic;

namespace Ayumi.Extension {
    [Obsolete("It's used in .Net 2 btw. So now it's not needed anymore")]
    public static class EnumerableExt {
        //public delegate TResult Func<T, TResult>(T t);

        public static Boolean HaveAny<TSource>(this IEnumerable<TSource> source) {
            if (source == null)
                throw new ArgumentNullException("source");

            using (IEnumerator<TSource> e = source.GetEnumerator())
                return e.MoveNext();
        }

        public static Boolean HaveAny<TSource>(this IEnumerable<TSource> source, Func<TSource, Boolean> predicate) {
            if (source == null)
                throw new ArgumentNullException("source");

            return HaveAny(Filter(source, predicate));
        }

        public static Int32 HaveCount<TSource>(this IEnumerable<TSource> source) {
            if (source == null)
                throw new ArgumentNullException("source");

            Int32 counter = 0;
            using (IEnumerator<TSource> e = source.GetEnumerator())
                while (e.MoveNext())
                    counter++;

            return counter;
        }

        public static Int32 HaveCount<TSource>(this IEnumerable<TSource> source, Func<TSource, Boolean> predicate) {
            if (source == null)
                throw new ArgumentNullException("source");

            IEnumerable<TSource> predicateResult = Filter(source, predicate);
            return HaveCount(predicateResult);
        }

        public static TSource GetFirstOrDefault<TSource>(this IEnumerable<TSource> source) {
            if (source == null)
                throw new ArgumentNullException("source");

            using (IEnumerator<TSource> e = source.GetEnumerator())
                return e.MoveNext() ? e.Current : default(TSource);
        }

        public static TSource GetFirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, Boolean> predicate) {
            if (source == null)
                throw new ArgumentNullException("source");

            return GetFirstOrDefault(Filter(source, predicate));
        }

        public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source, Func<TSource, Boolean> predicate) {
            if (source == null)
                throw new ArgumentNullException("source");

            using (IEnumerator<TSource> e = source.GetEnumerator())
                while (e.MoveNext())
                    if (predicate(e.Current))
                        yield return e.Current;
        }

        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> predicate) {
            if (source == null)
                throw new ArgumentNullException("source");

            using (IEnumerator<TSource> e = source.GetEnumerator())
                while (e.MoveNext())
                    yield return predicate(e.Current);
        }

        public static Boolean HaveContains<TSource>(this IEnumerable<TSource> source, TSource value) {
            if (source == null)
                throw new ArgumentNullException("source");

            IEqualityComparer<TSource> comparer = EqualityComparer<TSource>.Default;
            return HaveAny(source, delegate (TSource item) { return comparer.Equals(item, value); });
        }

        public static IEnumerable<TSource> GetOrderedBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => GetOrderedBy(source, keySelector, false);

        public static IEnumerable<TSource> GetOrderedBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Boolean isDescending) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            TSource[] sourceList = AsAnArray(source);
            var keyList = new TKey[sourceList.Length];
            for (Int32 i = 0; i < sourceList.Length; i++)
                keyList[i] = keySelector(sourceList[i]);

            Int32 orderDirection = isDescending ? -1 : 1;
            Comparer<TKey> keyComparer = Comparer<TKey>.Default;

            for (Int32 i = 0; i < sourceList.Length; i++) {
                for (Int32 j = 0; j < i; j++) {
                    Int32 result = keyComparer.Compare(keyList[i], keyList[j]) * orderDirection;
                    if (result < 0) {
                        TSource temp = sourceList[i];
                        sourceList[i] = sourceList[j];
                        sourceList[j] = temp;

                        TKey tempK = keyList[i];
                        keyList[i] = keyList[j];
                        keyList[j] = tempK;
                    }
                }
            }

            // to force array to IEnumerable
            foreach (TSource s in sourceList)
                yield return s;
        }

        public static List<TSource> AsAList<TSource>(this IEnumerable<TSource> source) {
            if (source == null)
                throw new ArgumentNullException("source");

            return new List<TSource>(source);
        }

        public static TSource[] AsAnArray<TSource>(this IEnumerable<TSource> source) {
            if (source == null)
                throw new ArgumentNullException("source");

            Int32 sourceCount = HaveCount(source);
            var arrayResult = new TSource[sourceCount];
            var convertedSource = source as IList<TSource>;
            for (Int32 idx = 0; idx < sourceCount; idx++)
                arrayResult[idx] = convertedSource[idx];

            return arrayResult;
        }

        public static IEnumerable<TResult> CastTo<TResult>(this IEnumerable source) {
            if (source == null)
                throw new ArgumentNullException("source");

            foreach (Object current in source)
                yield return (TResult) current;
        }
    }
}