using System;
using System.Collections.Generic;
using System.Linq;
using Ayumi.Data.Db;
using Ayumi.Data.Query;

namespace Ayumi.Extension {
    public static class IDatabaseExt {
        public static T GetById<T>(this IDatabase db, T data) where T : class {
            String query = data.CreateGetByIdQuery();
            using (db) {
                db.
            }



                var commandData = GetSelectByIdQuery(data);
            FillSqlCommand(sqlCommand, commandData);

            T returnData;

            using (SqlDataReader dr = sqlCommand.ExecuteReader())
                returnData = DataReaderUtility.ReadSingleDataFromDr<T>(dr);

            if (!Equals(returnData, default(T)))
                SetDetailValues(sqlCommand, data, returnData);

            return returnData;
        }

        static String CreateGetByIdQuery<T>(this T data) where T : class {
            TableAttribute tableInfo = data
                .GetDecorators<TableAttribute, T>()
                .Single();

            String predicate = String.Join(" AND ", data
                    .GetColumnInfos()
                    .Where(info => info.Column.PrimaryKey)
                    .Select(col => $"{col.Column.Name} = @{col.Property.Name}")
                );

            return new SqlBuilder()
                .SelectAll()
                .From(tableInfo.Name)
                .Where(predicate)
                .ToString();
        }

        static CommandData GetSelectByIdQuery<T>(T dataObject) where T : new() {
            var pkParams = ReflectionUtility.GetFieldsWithParameter<T>(DbFieldType.PRIMARY_KEY);
            var tableName = ReflectionUtility.GetTableName<T>();

            var query = new SqlBuilder()
                .SelectAll()
                .From(tableName)
                .Where(SqlBuilder.CombineWithAnd(pkParams))
                .ToString();

            return CommandData.CreateObject(query, dataObject);
        }

        public static void SaveDetails<THead, TDet>(SqlCommand sqlCommand, THead dataHeader, IEnumerable<TDet> details)
                    where THead : new()
            where TDet : new() {
            DeleteDetailsByHeader<THead, TDet>(sqlCommand, dataHeader);

            foreach (TDet dataDetail in details)
                SaveDetail(sqlCommand, dataHeader, dataDetail);
        }

        //todo: make recursive
        static void SaveDetail<THead, TDet>(SqlCommand sqlCommand, THead dataHeader, TDet dataDetail)
            where THead : new()
            where TDet : new() {
            var commandData = SqlQueryUtility.GetInsertDetailCommand(dataHeader, dataDetail);
            ExecuteNonQuery(sqlCommand, commandData);

            //SaveAllItems(sqlCommand, dataDetail);
        }

        public static void DeleteDetailsByHeader<THead, TDet>(SqlCommand sqlCommand, THead dataHeader)
            where THead : new()
            where TDet : new() {
            var commandData = SqlQueryUtility.GetDeleteByHeaderCommand<THead, TDet>(dataHeader);
            ExecuteNonQuery(sqlCommand, commandData);
        }

        //todo: make recursive
        public static IEnumerable<TDet> SelectDetailsByHeader<THead, TDet>(SqlCommand sqlCommand, THead dataHeader)
            where THead : new()
            where TDet : new() {
            var commandData = SqlQueryUtility.GetSelectByHeaderQuery<THead, TDet>(dataHeader);
            FillSqlCommand(sqlCommand, commandData);

            IEnumerable<TDet> returnData;
            using (SqlDataReader dr = sqlCommand.ExecuteReader())
                returnData = DataReaderUtility.ReadMultipleDataFromDr<TDet>(dr);

            //foreach (var data in returnData)
            //    SetDetailValues(sqlCommand, data, data);

            return returnData;
        }

        static void ExecuteNonQuery(SqlCommand sqlCommand, CommandData commandData) {
            FillSqlCommand(sqlCommand, commandData);
            sqlCommand.ExecuteNonQuery();
        }

        public static void Insert<T>(SqlCommand sqlCommand, T dataObject)
            where T : new() {
            var commandData = SqlQueryUtility.GetInsertCommand(dataObject);
            ExecuteNonQuery(sqlCommand, commandData);

            SaveAllItems(sqlCommand, dataObject);
        }

        static void SaveAllItems<T>(SqlCommand sqlCommand, T dataObject) where T : new() {
            var detailFields = ReflectionUtility.GetItemFieldsFromHeader(dataObject);

            foreach (Object item in detailFields) {
                Type itemType = item.GetType().GetGenericArguments()[0];
                Type[] genericArgs = new[] { typeof(T), itemType };
                var parameters = new[] { sqlCommand, dataObject, item };

                ReflectionUtility.InvokeGenericStaticMethod(typeof(WkEntitySql), "SaveDetails", genericArgs, parameters);
            }
        }

        public static void Update<T>(SqlCommand sqlCommand, T dataObject)
            where T : new() {
            var commandData = SqlQueryUtility.GetUpdateCommand(dataObject);
            ExecuteNonQuery(sqlCommand, commandData);

            SaveAllItems(sqlCommand, dataObject);
        }

        public static void Delete<T>(SqlCommand sqlCommand, T dataObject)
            where T : new() {
            DeleteAllDetails(sqlCommand, dataObject);

            var commandData = SqlQueryUtility.GetDeleteCommand(dataObject);
            ExecuteNonQuery(sqlCommand, commandData);
        }

        //todo: make recursive
        static void DeleteAllDetails<T>(SqlCommand sqlCommand, T dataHeader)
            where T : new() {
            var detailFields = ReflectionUtility.GetItemFields<T>();

            foreach (var item in detailFields) {
                var itemType = item.PropertyType.GetGenericArguments()[0];
                InvokeDeleteByHeader(sqlCommand, dataHeader, itemType);

                //MethodInfo method = typeof(ReflectionUtility).GetMethod("GetItemFields");
                //MethodInfo generic = method.MakeGenericMethod(itemType);
                //var itemItems = (IEnumerable<PropertyInfo>) generic.Invoke(null, null);

                //foreach (var itemItem in itemItems)
                //{
                //    InvokeDeleteByHeader(sqlCommand, dataHeader, itemItem.PropertyType);
                //}
            }
        }

        static void InvokeDeleteByHeader<T>(SqlCommand sqlCommand, T dataHeader, Type itemType) where T : new() {
            Type[] genericArgs = new[] { typeof(T), itemType };
            Object[] parameters = new[] { sqlCommand, dataHeader };

            ReflectionUtility.InvokeGenericStaticMethod(typeof(WkEntitySql), "DeleteDetailsByHeader", genericArgs, parameters);
        }

        public static Boolean IsExist<T>(SqlCommand sqlCommand, T dataWithKey)
            where T : new() {
            T data = GetById(sqlCommand, dataWithKey);

            if (Equals(data, default(T)))
                return false;

            return true;
        }

        public static void Save<T>(SqlCommand sqlCommand, T dataObject)
            where T : new() {
            if (!IsExist(sqlCommand, dataObject))
                Insert(sqlCommand, dataObject);

            else
                Update(sqlCommand, dataObject);
        }
        static void SetDetailValues<T>(SqlCommand sqlCommand, T dataHeader, T returnData) where T : new() {
            var detailFields = ReflectionUtility.GetItemFields<T>();

            foreach (var item in detailFields) {
                var itemType = item.PropertyType.GetGenericArguments()[0];
                Type[] genericArgs = new[] { typeof(T), itemType };
                var parameters = new Object[] { sqlCommand, dataHeader };

                var detailValue = ReflectionUtility.InvokeGenericStaticMethod(typeof(WkEntitySql), "SelectDetailsByHeader", genericArgs, parameters);

                item.SetValue(returnData, detailValue, null);
            }
        }

        public static IEnumerable<T> SelectWithWhereClause<T>(SqlCommand sqlCommand, String whereClause)
            where T : new() {
            sqlCommand.CommandText = SqlQueryUtility.GetSelectWithWhereClauseQuery<T>(whereClause);

            using (SqlDataReader dr = sqlCommand.ExecuteReader())
                return DataReaderUtility.ReadMultipleDataFromDr<T>(dr);
        }

        static void FillSqlCommand(SqlCommand sqlCommand, CommandData commandData) {
            sqlCommand.CommandText = commandData.CommandString;
            PopulateSqlCommandParameters(sqlCommand, commandData);
        }

        static void PopulateSqlCommandParameters(SqlCommand sqlCommand, CommandData commandData) {
            sqlCommand.Parameters.Clear();

            foreach (var param in commandData.ParameterList)
                sqlCommand.Parameters.AddWithValue(param.Key, param.Value);
        }
    }
}