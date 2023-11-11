using System;
using System.Collections.Generic;
using System.Linq;
using Ayumi.Data;

namespace Ayumi.Extension {
    public static class ISelectListExt {
        public static void BindToISelectList<T>(this ISelectList selectList, IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class =>
            BindToISelectList(selectList, dataList, nameSelector, valueSelector, false);

        public static void BindToISelectList<T>(this ISelectList selectList, IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector, Boolean isOrdered) where T : class {
            IList<NameValueItem> nviList = dataList
                .Select(item => new NameValueItem(nameSelector(item), valueSelector(item)))
                .ToList();

            nviList.Insert(0, new NameValueItem(String.Empty, String.Empty));

            RawBindToISelectList(selectList, nviList, isOrdered);
        }

        public static void BindToISelectList(this ISelectList selectList, IEnumerable<NameValueItem> dataList) => BindToISelectList(selectList, dataList, false);

        public static void BindToISelectList(this ISelectList selectList, IEnumerable<NameValueItem> dataList, Boolean isOrdered) {
            IList<NameValueItem> nviList = dataList as IList<NameValueItem> ?? dataList.ToList();
            nviList.Insert(0, new NameValueItem(String.Empty, String.Empty));

            RawBindToISelectList(selectList, nviList, isOrdered);
        }

        public static void RawBindToISelectList<T>(this ISelectList selectList, IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class =>
            RawBindToISelectList(selectList, dataList, nameSelector, valueSelector, false);

        public static void RawBindToISelectList<T>(this ISelectList selectList, IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector, Boolean isOrdered) where T : class {
            selectList.Clear();

            if (isOrdered) {
                IEnumerable<NameValueItem> orderedList = dataList
                    .Select(item => new NameValueItem(nameSelector(item), valueSelector(item)))
                    .OrderBy(nvi => nvi.Name);

                selectList.AddRange(orderedList);
            }
            else {
                foreach (T item in dataList)
                    selectList.Add(new NameValueItem(nameSelector(item), valueSelector(item)));
            }
        }

        public static void RawBindToISelectList(this ISelectList selectList, IEnumerable<NameValueItem> dataList) => RawBindToISelectList(selectList, dataList, false);

        public static void RawBindToISelectList(this ISelectList selectList, IEnumerable<NameValueItem> dataList, Boolean isOrdered) {
            selectList.Clear();
            selectList.AddRange(isOrdered ? Enumerable.OrderBy(dataList, nvi => nvi.Name) : dataList);
        }
    }
}