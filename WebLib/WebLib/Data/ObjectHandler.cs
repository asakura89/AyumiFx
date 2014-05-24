using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

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
        /// <param name="listCondition">filter condition</param>
        /// <returns></returns>
        public static string GetSelectQueryBuilder<TSource>(List<ConditionData> listCondition)
        {
            List<ObjectRepoData> listObjectRepo = GetListObjectRepoFromObject<TSource>();
            TSource data = Activator.CreateInstance<TSource>();
            string queryBuild = "Select ";
            if(listObjectRepo.Count==1)
                queryBuild += " * ";
            for (int i = 1; i < listObjectRepo.Count; i++)
            {
                if (listObjectRepo[i].Name.Equals(ObjectRepoConstant.TABLE_NAME))
                    continue;
                Type type = data.GetType();
                PropertyInfo propInfo = type.GetProperty(listObjectRepo[i].Name);
                Type returnType = propInfo.PropertyType;
                if ((returnType.IsGenericType &&
            returnType.GetGenericTypeDefinition() == typeof(List<>)))
                    continue;
                queryBuild += listObjectRepo[i].Name;
                if (i != listObjectRepo.Count - 1)
                    queryBuild += ",";
            }
            
            queryBuild += " from " + listObjectRepo[0].Type + " where 1=1 ";
            if (listCondition != null && listCondition.Count != 0)
                queryBuild += GenerateConditionToQuery(listCondition);
            return queryBuild;
        }

        private static ObjectRepoData GetClassName<TSource>(TSource obj)
        {
            ObjectRepoData table = new ObjectRepoData();
            table.Name = ObjectRepoConstant.TABLE_NAME;
            table.Type = (object)obj.GetType().Name;
            return table;
        }

        private static string GenerateConditionToQuery(List<ConditionData> listCondition)
        {
            string conditionQuery = string.Empty;
            foreach (ConditionData cond in listCondition)
            {
                if(cond.Operator.Equals(OperatorConstant.OP_BETWEEN))
                {
                    conditionQuery += CreateConditionQueryBETWEEN(cond);
                }
                else if(cond.Operator.Equals(OperatorConstant.OP_IN))
                {
                    conditionQuery += CreateConditionQueryIN(cond);
                }
                else if (cond.Operator.Equals(OperatorConstant.OP_LIKE))
                {
                    conditionQuery += CreateConditionQueryLIKE(cond);
                }
                else
                {
                    conditionQuery += CreateConditionQueryNORMAL(cond);
                }
            }
            return conditionQuery;
        }

        private static string CreateConditionQueryNORMAL(ConditionData condData)
        {
            string query = string.Empty;
            query += condData.Connector + condData.ColumnName + condData.Operator
                + " '" + condData.Value[0] + "' ";
            return query;
        }

        private static string CreateConditionQueryLIKE(ConditionData condData)
        {
            string query = string.Empty;
            query += condData.Connector + condData.ColumnName + condData.Operator
                + " '%" + condData.Value[0] + "%' ";
            return query;
        }

        private static string CreateConditionQueryBETWEEN(ConditionData condData)
        {
            string query = string.Empty;
            query += condData.Connector + condData.ColumnName+  condData.Operator
                + " '" + condData.Value[0] + "' and "
                + " '" + condData.Value[1] + "'";
            return query;
        }

        private static string CreateConditionQueryIN(ConditionData condData)
        {
            string query = string.Empty;
            query += condData.Connector + condData.ColumnName + condData.Operator + " (";
            for (int i = 0; i < condData.Value.Length; i++)
            {
                query += "'"+ condData.Value[i]+"'";
                if (i != condData.Value.Length - 1)
                    query += ", ";
            }
            query += ") ";
            return query;
        }


       
        
    }
}
