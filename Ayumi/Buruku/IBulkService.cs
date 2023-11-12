using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Buruku {
    public interface IBulkService {
        IBulkService ForTable(String tableName);
        IBulkService UseConnectionString(String connectionString);
        IBulkService UseSqlConnection(SqlConnection connection);
        void Insert<T>(IEnumerable<T> dataList) where T : class;
    }
}