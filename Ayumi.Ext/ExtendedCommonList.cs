using System;
using System.Collections.Generic;
using System.Linq;
using Ayumi.Data;

namespace Ayumi.Extension
{
    public static class ExtendedCommonList
    {
        public static void BindToICommonList<T>(this ISelectList selectList, IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class
        {
            BindToICommonList(selectList, dataList, nameSelector, valueSelector, false);
        }
        public static void BindToICommonList<T>(this ISelectList selectList, IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector, Boolean isOrdered) where T : class
        {
            IList<NameValueItem> nviList = Enumerable.ToList(Enumerable.Select(dataList, item => new NameValueItem(nameSelector(item), valueSelector(item))));
            nviList.Insert(0, new NameValueItem(String.Empty, String.Empty));

            RawBindToICommonList(selectList, nviList, isOrdered);
        }

        public static void BindToICommonList(this ISelectList selectList, IEnumerable<NameValueItem> dataList)
        {
            BindToICommonList(selectList, dataList, false);
        }
        public static void BindToICommonList(this ISelectList selectList, IEnumerable<NameValueItem> dataList, Boolean isOrdered)
        {
            IList<NameValueItem> nviList = dataList as IList<NameValueItem> ?? Enumerable.ToList(dataList);
            nviList.Insert(0, new NameValueItem(String.Empty, String.Empty));

            RawBindToICommonList(selectList, nviList, isOrdered);
        }

        public static void RawBindToICommonList<T>(this ISelectList selectList, IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class
        {
            RawBindToICommonList(selectList, dataList, nameSelector, valueSelector, false);
        }
        public static void RawBindToICommonList<T>(this ISelectList selectList, IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector, Boolean isOrdered) where T : class
        {
            selectList.Clear();

            if (isOrdered)
            {
                IEnumerable<NameValueItem> orderedList = Enumerable.OrderBy(Enumerable.Select(dataList, item => new NameValueItem(nameSelector(item), valueSelector(item))), nvi => nvi.Name);
                selectList.AddRange(orderedList);
            }
            else
            {
                foreach (T item in dataList)
                    selectList.Add(new NameValueItem(nameSelector(item), valueSelector(item)));
            }
        }

        public static void RawBindToICommonList(this ISelectList selectList, IEnumerable<NameValueItem> dataList)
        {
            RawBindToICommonList(selectList, dataList, false);
        }
        public static void RawBindToICommonList(this ISelectList selectList, IEnumerable<NameValueItem> dataList, Boolean isOrdered)
        {
            selectList.Clear();
            selectList.AddRange(isOrdered ? Enumerable.OrderBy(dataList, nvi => nvi.Name) : dataList);
        }
    }
}