using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using WebLib.Constant;

namespace WebLib.Data
{
    public static class ObjectHandler
    {
        /// <summary>
        /// Create Array of SqlParameter from Object
        /// </summary>
        /// <typeparam name="TSource">Type of Object</typeparam>
        /// <param name="data">Source of SqlParameter value</param>
        /// <param name="statusSP">Type Of Param. Using SPStatus constant</param>
        /// <returns>SqlParameter[]</returns>
        public static SqlParameter[] GetListParamForSP<TSource>(TSource data, string statusSP)
        {
            List<ObjectRepoData> listObject = GetListObjectRepoFromObject<TSource>();
            SqlParameter[] param = new SqlParameter[listObject.Count];
            param[0] = new SqlParameter(ObjectRepoConstant.PROC_STATUS, statusSP);
            int counter = 1;
            foreach (ObjectRepoData obj in listObject)
            {
                if (obj.Name.Equals(ObjectRepoConstant.TABLE_NAME))
                    continue;
                Type type = data.GetType();
                PropertyInfo propInfo = type.GetProperty(obj.Name);
                Type returnType = propInfo.PropertyType;
                if ((returnType.IsGenericType &&
            returnType.GetGenericTypeDefinition() == typeof(List<>)))
                    continue;
                if((returnType.Name=="DateTime"))
                    param[counter] = new SqlParameter(obj.Name, CommonFunction.ToSQLCompatibleFormat((DateTime)propInfo.GetValue(data, null)));
                else
                param[counter] = new SqlParameter(obj.Name, data.GetType().GetProperty(obj.Name).GetValue(data, null));
                counter++;
            }
            return param;

        }

        public static SqlParameter[] GetListParamForSP<TSource>(TSource data)
        {
            List<ObjectRepoData> listObject = GetListObjectRepoFromObject<TSource>();
            SqlParameter[] param = new SqlParameter[listObject.Count];
            int counter = 0;
            foreach (ObjectRepoData obj in listObject)
            {
                if (obj.Name.Equals(ObjectRepoConstant.TABLE_NAME))
                    continue;
                if (obj.Name.Equals(ObjectRepoConstant.TABLE_NAME))
                    continue;
                Type type = data.GetType();
                PropertyInfo propInfo = type.GetProperty(obj.Name);
                Type returnType = propInfo.PropertyType;
                if ((returnType.IsGenericType &&
            returnType.GetGenericTypeDefinition() == typeof(List<>)))
                    continue;
                param[counter] = new SqlParameter(obj.Name, data.GetType().GetProperty(obj.Name).GetValue(data, null));
                counter++;
            }
            return param;

        }

        /// <summary>
        /// Get List Property Of Class. The First Line is The Name of Class.
        /// Name of Class = Table Name, propertyName = Column Name
        /// </summary>
        /// <typeparam name="TSource">type of Object</typeparam>
        /// <returns>List<ObjectRepoData></returns>
        public static List<ObjectRepoData> GetListObjectRepoFromObject<TSource>()
        {
            TSource obj = Activator.CreateInstance<TSource>();
            List<ObjectRepoData> listObject = new List<ObjectRepoData>();
            listObject.Add(GetClassName<TSource>(obj));
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                ObjectRepoData attClass = new ObjectRepoData();
                attClass.Name = prop.Name;
                Type returnType = prop.PropertyType;
                if (returnType.IsGenericType &&
                   returnType.GetGenericTypeDefinition() == typeof(List<>))
                    attClass.Type = "List";
                else
                 attClass.Type = prop.PropertyType.Name;
                listObject.Add(attClass);
            }
            return listObject;
        }
        /// <summary>
        /// Get Query Select of Object 
        /// </summary>
        /// <typeparam name="TSource">Type of Object</typeparam>
        /// <param name="conditionList">filter condition</param>
        /// <returns></returns>
        public static string GetSelectQuery<TSource>(IEnumerable<Condition> conditionList)
        {
            var queryBuilder = new StringBuilder();
            var obj = Activator.CreateInstance<TSource>();
            Type objType = obj.GetType();
            PropertyInfo[] propertyList = objType.GetProperties();

            queryBuilder.Append("SELECT ");
            if (propertyList.Length <= 1)
                queryBuilder.Append("* ");
            else
            {
                var columnList = new String[propertyList.Length];
                for (int idx = 0; idx < propertyList.Length; idx++)
                {
                    PropertyInfo property = propertyList[idx];
                    Type propertyType = property.PropertyType;
                    if (!(propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof (List<>)))
                    {
                        Object[] attributeList = property.GetCustomAttributes(typeof (Column), false);
                        if (attributeList.Length > 0)
                        {
                            var attribute = (Column) attributeList[0];
                            if (!String.IsNullOrEmpty(attribute.ColumnName))
                                columnList[idx] = attribute.ColumnName;
                        }
                        else
                            columnList[idx] = property.Name;
                    }
                }

                String columnListString = String.Join(", ", columnList) + " ";
                queryBuilder.Append(columnListString);
            }

            queryBuilder.Append("FROM " + objType.Name + " WHERE 1=1 ");
            if (conditionList != null && CollectionExtension.Any(conditionList))
                queryBuilder.Append(GenerateConditionToQuery(conditionList));

            return queryBuilder.ToString();
        }

        private static ObjectRepoData GetClassName<TSource>(TSource obj)
        {
            ObjectRepoData table = new ObjectRepoData();
            table.Name = ObjectRepoConstant.TABLE_NAME;
            table.Type = (object)obj.GetType().Name;
            return table;
        }

        public static string GenerateConditionToQuery(IEnumerable<Condition> listCondition)
        {
            String conditionQuery = String.Empty;
            foreach (Condition cond in listCondition)
            {
                switch (cond.Operator)
                {
                    case Operator.Between:
                        conditionQuery += CreateConditionQueryBETWEEN(cond);
                        break;
                    case Operator.In:
                        conditionQuery += CreateConditionQueryIN(cond);
                        break;
                    case Operator.Like:
                        conditionQuery += CreateConditionQueryLIKE(cond);
                        break;
                    case Operator.Is:
                        conditionQuery += CreateConditionQueryIS(cond);
                        break;
                    default:
                        conditionQuery += CreateConditionQueryNORMAL(cond);
                        break;
                }
            }

            return conditionQuery;
        }

        private static string CreateConditionQueryNORMAL(Condition cond)
        {
            string query = string.Empty;
            query += cond.Connector + cond.ColumnName + cond.Operator
                + " '" + cond.ColumnValue[0] + "' ";
            return query;
        }

        private static string CreateConditionQueryIS(Condition cond)
        {
            string query = string.Empty;
            query += cond.Connector + cond.ColumnName + cond.Operator
                + " " + cond.ColumnValue[0] + " ";
            return query;
        }

        private static string CreateConditionQueryLIKE(Condition cond)
        {
            string query = string.Empty;
            query += cond.Connector + cond.ColumnName + cond.Operator
                + " '%" + cond.ColumnValue[0] + "%' ";
            return query;
        }

        private static string CreateConditionQueryBETWEEN(Condition cond)
        {
            string query = string.Empty;
            query += cond.Connector + cond.ColumnName+  cond.Operator
                + " '" + cond.ColumnValue[0] + "' and "
                + " '" + cond.ColumnValue[1] + "'";
            return query;
        }

        private static string CreateConditionQueryIN(Condition cond)
        {
            string query = string.Empty;
            query += cond.Connector + cond.ColumnName + cond.Operator + " (";
            for (int i = 0; i < cond.ColumnValue.Length; i++)
            {
                query += "'"+ cond.ColumnValue[i]+"'";
                if (i != cond.ColumnValue.Length - 1)
                    query += ", ";
            }
            query += ") ";
            return query;
        }
    }
}
