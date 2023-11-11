using System;
using System.Data;
using System.Data.SqlClient;

namespace WK.AppUtility
{
    internal sealed class TxManager
    {
        private readonly SqlConnection sqlConnection;
        private SqlTransaction transaction;
        private int txBeginCount;
        private int openConnectionCount;

        public TxManager(SqlConnection connection)
        {
            sqlConnection = connection;
        }

        public void OpenConnection()
        {
            if (sqlConnection.State != ConnectionState.Open && openConnectionCount == 0)
                sqlConnection.Open();

            openConnectionCount++;
        }

        public void ReleaseConnection()
        {
            if (openConnectionCount == 0) return;
            openConnectionCount--;

            CloseConnection();
        }

        private void CloseConnection()
        {
            if (openConnectionCount != 0) return;

            CleanupTransaction();

            if (sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        private void SetTransaction(SqlCommand cmd)
        {
            if (transaction != null)
                cmd.Transaction = transaction;
        }

        public SqlCommand CreateCommand()
        {
            SqlCommand cmd = sqlConnection.CreateCommand();
            SetTransaction(cmd);
            return cmd;
        }

        public void BeginTransaction()
        {
            if (txBeginCount == 0)
                transaction = sqlConnection.BeginTransaction();

            txBeginCount++;
        }

        public void CommitTransaction()
        {
            if (txBeginCount == 0)
                throw new Exception("Invalid commit operation.");

            txBeginCount--;
            if (txBeginCount == 0)
                transaction.Commit();
        }

        void RollBackTransaction()
        {
            transaction.Rollback();
            txBeginCount = 0;
        }

        void CleanupTransaction()
        {
            if (transaction == null) return;

            if (txBeginCount != 0)
                RollBackTransaction();

            transaction.Dispose();
            transaction = null;
        }
    }
}