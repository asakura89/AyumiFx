using System;
using System.Collections.Generic;
using System.Linq;

namespace Nvy {
    public static class NameValueItemExt {
        public static IEnumerable<NameValueItem> AsNameValueList(this String delimitedString) {
            String[] splittedList = delimitedString.Split(NameValueItem.ListDelimiter);
            return splittedList
                .Select(item => item.Split(NameValueItem.ItemDelimiter))
                .Select(splittedItem => new NameValueItem(splittedItem[0], splittedItem[1]));
        }

        public static IEnumerable<NameValueItem> AsNameValueList<T>(this IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class =>
            dataList.Select(data => new NameValueItem(nameSelector(data), valueSelector(data)));
    }
}