using System;
using System.Collections.Generic;
using System.Reflection;
using EnumExperimentation;
using WebLib.Data;

namespace WebLib
{
    public static class EnumExtension
    {
        // http://stackoverflow.com/questions/424366/c-sharp-string-enums
        // http://blog.spontaneouspublicity.com/associating-strings-with-enums-in-c
        public static String GetStringValue(System.Enum enumField)
        {
            String returnedString;
            FieldInfo fieldInfo = enumField.GetType().GetField(enumField.ToString());

            var attrs = fieldInfo.GetCustomAttributes(typeof(StringValue), false) as StringValue[];

            if (attrs != null && attrs.Length > 0)
                returnedString = attrs[0].Value;
            else
                returnedString = enumField.ToString();

            return returnedString;
        }

        public static IEnumerable<String> ToStringValueArray(Type enumTypeOf)
        {
            if (!enumTypeOf.IsEnum)
                throw new ArgumentException("Must be an Enum");

            // Enum base type has Int32 public field used for enum value
            IEnumerable<FieldInfo> fieldInfos = CollectionExtension.Where(enumTypeOf.GetFields(),
                delegate(FieldInfo field) { return field.FieldType != typeof (Int32); });
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
                if (attrs != null && attrs.Length > 0)
                    yield return attrs[0].Value;
                else
                    yield return fieldInfo.GetValue(fieldInfo.FieldType).ToString();
            }
        }

        public static IEnumerable<String> GetEnumNames(Type enumTypeOf)
        {
            if (!enumTypeOf.IsEnum)
                throw new ArgumentException("Must be an Enum");

            FieldInfo[] fieldInfos = enumTypeOf.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
                yield return fieldInfo.Name;
        }

        public static IEnumerable<System.Enum> ToArray(Type enumTypeOf)
        {
            if (!enumTypeOf.IsEnum)
                throw new ArgumentException("Must be an Enum");

            IEnumerable<System.Enum> enumNames = CollectionExtension.Select(GetEnumNames(enumTypeOf),
                delegate(String enumName) { return (System.Enum) System.Enum.Parse(enumTypeOf, enumName); });

            return enumNames;
        }

        public static System.Enum ToEnum(Int32 intValue, Type enumTypeOf)
        {
            if (!enumTypeOf.IsEnum)
                throw new ArgumentException("Must be an Enum");

            return (System.Enum) System.Enum.ToObject(enumTypeOf, intValue);
        }

        public static System.Enum ParseStringValue(Type enumTypeOf, String stringValue, Boolean ignoreCase)
        {
            if (!enumTypeOf.IsEnum)
                throw new ArgumentException("Must be an Enum");

            System.Enum matchEnum = null;
            String enumStringValue = String.Empty;
            FieldInfo[] fieldInfos = enumTypeOf.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
                if (attrs != null && attrs.Length > 0)
                    enumStringValue = attrs[0].Value;

                if (String.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    matchEnum = (System.Enum) System.Enum.Parse(enumTypeOf, fieldInfo.Name);
                    break;
                }
            }

            return matchEnum;
        }
    }
}