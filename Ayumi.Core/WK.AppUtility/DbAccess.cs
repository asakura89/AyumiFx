using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WK.DBUtility;

namespace WK.AppUtility
{
    public sealed class DbAccess : IDisposable
    {
        public DbAccess()
        {
            DatabaseManager.GlobalTx.OpenConnection();
        }

        public void Dispose()
        {
            DatabaseManager.GlobalTx.ReleaseConnection();
        }

        public void BeginTransaction()
        {
            DatabaseManager.GlobalTx.BeginTransaction();
        }

        public void CommitTransaction()
        {
            DatabaseManager.GlobalTx.CommitTransaction();
        }

        public SqlCommand CreateCommand()
        {
            return DatabaseManager.GlobalTx.CreateCommand();
        }
    }
}
