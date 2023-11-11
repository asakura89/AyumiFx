using System.Collections.Generic;
using System.Data.SqlClient;

namespace WK.DBUtility
{
    static class SqlCommandExtension
    {
        public static T SelectById<T>(this SqlCommand command, T dataWithKey) 
            where T:new()
        {
            return WkEntitySql.SelectById(command, dataWithKey);
        }

        public static IEnumerable<T> SelectWithWhereClause<T>(this SqlCommand command, string whereClause) 
            where T : new()
        {
            return WkEntitySql.SelectWithWhereClause<T>(command, whereClause);
        }

        public static bool IsDataExist<T>(this SqlCommand command, T dataWithKey) 
            where T : new()
        {
            return WkEntitySql.IsExist(command, dataWithKey);
        }

        public static void Save<T>(this SqlCommand command, T data) 
            where T:new()
        {
            WkEntitySql.Save(command, data);
        }

        public static void Delete<T>(this SqlCommand command, T dataWithKey) 
            where T : new()
        {
            WkEntitySql.Delete(command, dataWithKey);
        }

        public static void Update<T>(this SqlCommand command, T data) 
            where T : new()
        {
            WkEntitySql.Update(command, data);
        }

        public static void Insert<T>(this SqlCommand command, T data) 
            where T : new()
        {
            WkEntitySql.Insert(command, data);
        }
    }
}
