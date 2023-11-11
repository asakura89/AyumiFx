using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WK.DBUtility
{
    static class ReflectionUtility
    {
        public static List<object> GetItemFieldsFromHeader<T>(T dataObject) where T : new()
        {
            IEnumerable<PropertyInfo> itemFields = GetItemFields<T>();
            return itemFields.Select(field => field.GetValue(dataObject, null)).ToList();
        }

        public static string GetTableName<T>() where T : new()
        {
            object[] custAttr = typeof(T).GetCustomAttributes(typeof(WkTableAttribute), false);

            if (custAttr.Length == 0)
                throw new Exception("DbTable attributeType is not set.");

            var dClass = (WkTableAttribute)custAttr[0];
            return dClass.TableName;
        }

        public static string GetDbFieldName(PropertyInfo field)
        {
            string fieldName = GetFieldAttributeInfo(field).FieldName;
            if (fieldName == String.Empty) fieldName = field.Name;
            return fieldName;
        }

        static string GetDbFieldParamName(PropertyInfo field)
        {
            return String.Format("@{0}", GetDbFieldName(field));
        }

        public static List<PropertyInfo> GetDbFieldsNotJoin<T>() where T : new()
        {
            var notJoinProperties = from field in GetAllDbFields<T>()
                let attrInfo = GetFieldAttributeInfo(field)
                where attrInfo.FieldType != DbFieldType.JOIN_FIELD
                select field;
            return notJoinProperties.ToList();
        }

        public static List<PropertyInfo>  GetAllDbFields<T>() where T:new()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            List<PropertyInfo> allDbFields = FilterFieldByAttribute(properties, typeof(WkFieldAttribute)).ToList();
            return allDbFields;
        }

        public static IEnumerable<PropertyInfo> GetItemFields<T>() where T : new()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return FilterFieldByAttribute(properties, typeof(WkItemFieldAttribute)).ToList();
        }

        static IEnumerable<PropertyInfo> FilterFieldByAttribute(IEnumerable<PropertyInfo> properties, Type attributeType)
        {
            return (from pInfo in properties
                let custAttr = pInfo.GetCustomAttributes(attributeType, true)
                where custAttr.Length >= 1
                select pInfo).ToList();
        }

        static WkFieldAttribute GetFieldAttributeInfo(PropertyInfo pInfo)
        {
            object[] custAttr = pInfo.GetCustomAttributes(typeof(WkFieldAttribute), true);

            if (custAttr.Length == 0)
                throw new Exception("DbField attributeType is not set.");

            var attrInfo = (WkFieldAttribute)custAttr[0];
            return attrInfo;
        }

        static IEnumerable<PropertyInfo> FilterFields<T>(DbFieldType fieldType) where T : new()
        {
            return (from field in GetDbFieldsNotJoin<T>()
                let attrInfo = GetFieldAttributeInfo(field)
                where attrInfo.FieldType == fieldType
                select field).ToList();
        }

        public static List<string> GetFieldsWithParameter<T>(DbFieldType dbFieldType) where T : new()
        {
            return (from fields in FilterFields<T>(dbFieldType)
                let fieldName = GetDbFieldName(fields)
                let fieldParamName = GetDbFieldParamName(fields)
                select String.Format("{0} = {1}", fieldName, fieldParamName)).ToList();
        }

        public static List<string> GetDbFieldNameListNotJoin<T>(DbFieldType dbFieldType) where T : new()
        {
            return GetDbFieldsNotJoin<T>().Where(x => GetFieldAttributeInfo(x).FieldType == dbFieldType).Select(GetDbFieldName).ToList();
        }

        public static List<string> GetDbFieldParameterNameListNotJoin<T>(DbFieldType dbFieldType) where T : new()
        {
            return GetDbFieldsNotJoin<T>().Where(x => GetFieldAttributeInfo(x).FieldType == dbFieldType).Select(GetDbFieldParamName).ToList();
        }

        public static List<string> GetAllFieldNamesNotJoin<T>() where T : new()
        {
            return GetDbFieldsNotJoin<T>().Select(GetDbFieldName).ToList();
        }

        public static List<string> GetAllParameterNamesNotJoin<T>() where T : new()
        {
            return GetDbFieldsNotJoin<T>().Select(GetDbFieldParamName).ToList();
        }

        public static Dictionary<string, object> GetAllParametersWithValue<T>(T dataObject) where T : new()
        {
            var parameters = from fields in GetDbFieldsNotJoin<T>()
                let fieldParamName = GetDbFieldParamName(fields)
                let fieldValue = fields.GetValue(dataObject, null)
                select new { fieldParamName, fieldValue };

            return parameters.ToDictionary(temp => temp.fieldParamName, temp => temp.fieldValue);
        }

        public static object InvokeGenericStaticMethod(Type classType, string staticMethodName, Type[] genericArgs, object[] parameters)
        {
            MethodInfo method = classType.GetMethod(staticMethodName);
            MethodInfo generic = method.MakeGenericMethod(genericArgs);

            var value = generic.Invoke(null, parameters);

            return value;
        }
    }
}
