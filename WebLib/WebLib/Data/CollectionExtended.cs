using System;
using System.Collections.Generic;

namespace WebLib.Data
{
    public static class CollectionExtended
    {
        public delegate TResult Func<T, TResult>(T t);

        public static Boolean Any<TSource>(IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            using (IEnumerator<TSource> e = source.GetEnumerator())
                return e.MoveNext();
        }

        public static Boolean Any<TSource>(IEnumerable<TSource> source, Func<TSource, Boolean> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");

            return Any(Where(source, predicate));
        }

        public static Int32 Count<TSource>(IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Int32 counter = 0;
            using (IEnumerator<TSource> e = source.GetEnumerator())
                while (e.MoveNext())
                    counter++;

            return counter;
        }

        public static TSource FirstOrDefault<TSource>(IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            using (IEnumerator<TSource> e = source.GetEnumerator())
                return e.MoveNext() ? e.Current : default(TSource);
        }

        public static TSource FirstOrDefault<TSource>(IEnumerable<TSource> source, Func<TSource, Boolean> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");

            return FirstOrDefault(Where(source, predicate));
        }

        public static IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, Boolean> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");

            using (IEnumerator<TSource> e = source.GetEnumerator())
                while (e.MoveNext())
                    if (predicate(e.Current))
                        yield return e.Current;
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");

            using (IEnumerator<TSource> e = source.GetEnumerator())
                while (e.MoveNext())
                    yield return predicate(e.Current);
        }

        public static Boolean Contains<TSource>(IEnumerable<TSource> source, TSource value)
        {
            if (source == null) throw new ArgumentNullException("source");

            IEqualityComparer<TSource> comparer = EqualityComparer<TSource>.Default;
            return Any(source, delegate(TSource item) { return comparer.Equals(item, value); });
        }

        public static List<TSource> ToList<TSource>(IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new List<TSource>(source);
        }
    } 
}
