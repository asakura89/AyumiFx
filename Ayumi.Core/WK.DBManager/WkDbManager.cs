using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WK.AppStatic;
using WK.DBUtility;

namespace WK.DBManager
{
    public sealed class WkDbManager : IDisposable
    {
        private readonly SqlConnection sqlConnection;
        private SqlTransaction transaction;
        private bool isThereUncommittedTransaction;

        public WkDbManager()
        {
            sqlConnection = new SqlConnection(AppGlobal.ConnectionString);
        }

        private void OpenConnection()
        {
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
        }

        public void Dispose()
        {
            CleanupTransaction();
            sqlConnection.Close();
            sqlConnection.Dispose();
        }

        private void SetTransaction(SqlCommand cmd)
        {
            if (transaction != null)
                cmd.Transaction = transaction;
        }

        SqlCommand CreateCommand()
        {
            OpenConnection();
            SqlCommand cmd = sqlConnection.CreateCommand();
            SetTransaction(cmd);
            return cmd;
        }

        public void BeginTransaction()
        {
            OpenConnection();
            transaction = sqlConnection.BeginTransaction();
            isThereUncommittedTransaction = true;
        }

        public void CommitTransaction()
        {
            transaction.Commit();
            isThereUncommittedTransaction = false;
        }

        void RollBackTransaction()
        {
            transaction.Rollback();
            isThereUncommittedTransaction = false;
        }

        void CleanupTransaction()
        {
            if (transaction == null) return;

            if (isThereUncommittedTransaction)
                RollBackTransaction();

            transaction.Dispose();
            transaction = null;
        }

        public void ExecuteInsert<T> (T dataObject) 
            where T: new()
        {
            using (var sqlCommand = CreateCommand())
                WkEntitySql.Insert(sqlCommand, dataObject);
        }

        public void ExecuteUpdate<T>(T dataObject)
            where T : new()
        {
            using (var sqlCommand = CreateCommand())
                WkEntitySql.Update(sqlCommand, dataObject);
        }

        public void ExecuteDelete<T>(T dataWithKey)
            where T : new()
        {
            using (var sqlCommand = CreateCommand())
                WkEntitySql.Delete(sqlCommand, dataWithKey);
        }

        public T SelectById<T>(T dataWithKey)
            where T : new()
        {
            using (var sqlCommand = CreateCommand())
                return WkEntitySql.SelectById(sqlCommand, dataWithKey);
        }

        public IEnumerable<T> SelectWithWhereClause<T>(string whereClause)
           where T : new()
        {
            using (var sqlCommand = CreateCommand())
                return WkEntitySql.SelectWithWhereClause<T>(sqlCommand, whereClause);
        }

        public void ExecuteSave<T>(T dataObject)
            where T:new()
        {
            using (var sqlCommand = CreateCommand())
                WkEntitySql.Save(sqlCommand, dataObject);
        }
    }
}
