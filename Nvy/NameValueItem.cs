using System;
using System.Collections.Generic;
using System.Linq;

namespace Nvy
{
    [Serializable]
    public class NameValueItem
    {
        public const String NameProperty = "Name";
        public const String ValueProperty = "Value";
        public const Char ListDelimiter = '·';
        public const Char ItemDelimiter = '•';

        public String Name { get; private set; }
        public String Value { get; private set; }

        public static NameValueItem Empty
        {
            get { return new NameValueItem(String.Empty, String.Empty); }
        }

        public static NameValueItem None
        {
            get { return new NameValueItem("None", String.Empty); }
        }

        public NameValueItem(String name, String value)
        {
            Name = name;
            Value = value;
        }

        public NameValueItem() : this(String.Empty, String.Empty) { }
    }

    public static class NameValueItemExtensions
    {
        public static IEnumerable<NameValueItem> AsNameValueList(this String delimitedString)
        {
            String[] splittedList = delimitedString.Split(NameValueItem.ListDelimiter);
            return splittedList
                .Select(item => item.Split(NameValueItem.ItemDelimiter))
                .Select(splittedItem => new NameValueItem(splittedItem[0], splittedItem[1]));
        }

        public static IEnumerable<NameValueItem> AsNameValueList<T>(this IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class
        {
            return dataList.Select(data => new NameValueItem(nameSelector(data), valueSelector(data)));
        }

        public static String AsDelimitedString(this IEnumerable<NameValueItem> nameValueList)
        {
            String[] delimitedStringList = nameValueList.Select(item => item.Name + NameValueItem.ItemDelimiter + item.Value).ToArray();
            String delimitedString = String.Join(NameValueItem.ListDelimiter.ToString(), delimitedStringList);

            return delimitedString;
        }

        public static String AsDelimitedString<T>(this IEnumerable<T> dataList, Func<T, String> nameSelector, Func<T, String> valueSelector) where T : class
        {
            return dataList.AsNameValueList(nameSelector, valueSelector).AsDelimitedString();
        }
    }
}
