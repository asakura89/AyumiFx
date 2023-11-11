using System;
using System.Collections.Generic;
using System.Linq;
using Ayumi.Data;

namespace Ayumi.Extension
{
    public static class ExtendedNameValueItem
    {
        public static IEnumerable<NameValueItem> AsNameValueList(this String delimitedString)
        {
            String[] splittedList = delimitedString.Split(NameValueItem.ListDelimiter);
            return Enumerable.Select(Enumerable.Select(splittedList, item => item.Split(NameValueItem.ItemDelimiter)), splittedItem => new NameValueItem(splittedItem[0], splittedItem[1]));
        }

        public static IEnumerable<NameValueItem> AsNameValueList<T>(this IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class
        {
            return Enumerable.Select(dataList, data => new NameValueItem(nameSelector(data), valueSelector(data)));
        }

        public static String AsDelimitedString(this IEnumerable<NameValueItem> nameValueList)
        {
            String[] delimitedStringList = Enumerable.ToArray(Enumerable.Select(nameValueList, item => item.Name + NameValueItem.ItemDelimiter + item.Value));
            String delimitedString = String.Join(NameValueItem.ListDelimiter.ToString(), delimitedStringList);

            return delimitedString;
        }

        public static String AsDelimitedString<T>(this IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class
        {
            return dataList.AsNameValueList(nameSelector, valueSelector).AsDelimitedString();
        }
    }
}