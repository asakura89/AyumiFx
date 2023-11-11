using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WK.AppStatic;

namespace WK.AppUtility
{
    internal class DatabaseManager
    {
        internal static void Init()
        {
            GlobalTx = new TxManager(new SqlConnection(AppGlobal.ConnectionString));
        }
        internal static TxManager GlobalTx;
    }
}
