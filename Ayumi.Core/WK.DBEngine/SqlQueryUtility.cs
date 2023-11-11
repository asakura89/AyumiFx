using System.Collections.Generic;
using System.Linq;

namespace WK.DBUtility
{
    static class SqlQueryUtility
    {
        public static CommandData GetSelectByIdQuery<T>(T dataObject) where T : new()
        {
            var pkParams = ReflectionUtility.GetFieldsWithParameter<T>(DbFieldType.PRIMARY_KEY);
            var tableName = ReflectionUtility.GetTableName<T>();

            var query = new SqlBuilder()
                .SelectAllFields()
                .From(tableName)
                .Where(SqlBuilder.CombineWithAnd(pkParams))
                .ToString();

            return CommandData.CreateObject(query, dataObject);
        }

        public static string GetSelectWithWhereClauseQuery<T>(string whereClause) where T: new()
        {
            var tableName = ReflectionUtility.GetTableName<T>();

            var query = new SqlBuilder()
                .SelectAllFields()
                .From(tableName)
                .Where(whereClause)
                .ToString();

            return query;
        }

        public static CommandData GetInsertCommand<T>(T dataObject) where T : new()
        {
            var fieldNameList = ReflectionUtility.GetAllFieldNamesNotJoin<T>();
            var fieldValueList = ReflectionUtility.GetAllParameterNamesNotJoin<T>();
            var tableName = ReflectionUtility.GetTableName<T>();

            var sqlCommandText = new SqlBuilder()
                .InsertInto(tableName)
                .Bracket(SqlBuilder.CombineWithComma(fieldNameList))
                .Values(SqlBuilder.CombineWithComma(fieldValueList))
                .ToString();

            return CommandData.CreateObject(sqlCommandText, dataObject);
        }

        public static CommandData GetDeleteCommand<T>(T dataObject) where T : new()
        {
            var pkParams = ReflectionUtility.GetFieldsWithParameter<T>(DbFieldType.PRIMARY_KEY);
            var tableName = ReflectionUtility.GetTableName<T>();

            var sqlCommandText = new SqlBuilder()
                .Delete(tableName)
                .Where(SqlBuilder.CombineWithAnd(pkParams))
                .ToString();

            return CommandData.CreateObject(sqlCommandText, dataObject);
        }

        public static CommandData GetUpdateCommand<T>(T dataObject) where T : new()
        {
            var nonPkParams = ReflectionUtility.GetFieldsWithParameter<T>(DbFieldType.NORMAL);
            var pkParams = ReflectionUtility.GetFieldsWithParameter<T>(DbFieldType.PRIMARY_KEY);
            var tableName = ReflectionUtility.GetTableName<T>();

            var sqlCommandText = new SqlBuilder()
                .Update(tableName)
                .Set(SqlBuilder.CombineWithComma(nonPkParams))
                .Where(SqlBuilder.CombineWithAnd(pkParams))
                .ToString();

            return CommandData.CreateObject(sqlCommandText, dataObject);
        }

        public static CommandData GetSelectByHeaderQuery<THead, TDet>(THead dataHeader)
            where THead : new()
            where TDet : new()
        {
            var tableName = ReflectionUtility.GetTableName<TDet>();
            var pkParams = ReflectionUtility.GetFieldsWithParameter<THead>(DbFieldType.PRIMARY_KEY);

            var query = new SqlBuilder()
                .SelectAllFields()
                .From(tableName)
                .Where(SqlBuilder.CombineWithAnd(pkParams))
                .ToString();

            return CommandData.CreateObject(query, dataHeader);
        }

        public static CommandData GetDeleteByHeaderCommand<THead, TDet>(THead dataHeader)
            where THead : new()
            where TDet : new()
        {
            var tableName = ReflectionUtility.GetTableName<TDet>();
            var pkParams = ReflectionUtility.GetFieldsWithParameter<THead>(DbFieldType.PRIMARY_KEY);

            var sqlCommandText = new SqlBuilder()
                .Delete(tableName)
                .Where(SqlBuilder.CombineWithAnd(pkParams))
                .ToString();

            return CommandData.CreateObject(sqlCommandText, dataHeader);
        }
      
        public static CommandData GetInsertDetailCommand<THead, TDet>(THead dataHeader, TDet dataDetail)
            where THead : new()
            where TDet : new()
        {
            var tableName = ReflectionUtility.GetTableName<TDet>();
            var allFieldName = GetDetailFieldNamesNotJoin<THead, TDet>();
            var allFieldParamNames = GetDetailFieldParamNamesNotJoin<THead, TDet>();

            var sqlCommandText = new SqlBuilder()
                .InsertInto(tableName)
                .Bracket(SqlBuilder.CombineWithComma(allFieldName))
                .Values(SqlBuilder.CombineWithComma(allFieldParamNames))
                .ToString();

            var commandData = GetCombinedCommandData(dataHeader, dataDetail, sqlCommandText);

            return commandData;
        }

        private static CommandData GetCombinedCommandData<THead, TDet>(THead dataHeader, TDet dataDetail, string sqlCommandText)
            where THead : new() 
            where TDet : new()
        {
            var commandData = CommandData.CreateObject(sqlCommandText, dataDetail);

            commandData.ParameterList =
                commandData.ParameterList.Union(CommandData.GetParametersForCommand(dataHeader, commandData.CommandString))
                    .ToDictionary(param => param.Key, param => param.Value);

            return commandData;
        }

        private static List<string> GetDetailFieldParamNamesNotJoin<THead, TDet>() 
            where THead : new() 
            where TDet : new() 
        {
            var pkHeaderParamNames = ReflectionUtility.GetDbFieldParameterNameListNotJoin<THead>(DbFieldType.PRIMARY_KEY);
            var fieldValueListDetail = ReflectionUtility.GetAllParameterNamesNotJoin<TDet>();
            var allFieldParamNames = pkHeaderParamNames.Union(fieldValueListDetail).ToList();

            return allFieldParamNames;
        }

        private static List<string> GetDetailFieldNamesNotJoin<THead, TDet>() 
            where THead : new() 
            where TDet : new()
        {
            var pkHeaderFieldNames = ReflectionUtility.GetDbFieldNameListNotJoin<THead>(DbFieldType.PRIMARY_KEY);
            var fieldNameListDetail = ReflectionUtility.GetAllFieldNamesNotJoin<TDet>();
            var allFieldNames = pkHeaderFieldNames.Union(fieldNameListDetail).ToList();

            return allFieldNames;
        }
    }
}
