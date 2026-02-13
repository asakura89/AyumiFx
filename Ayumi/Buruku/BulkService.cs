using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Buruku {
    public class BulkService : IBulkService {
        private String tableName;
        private String connectionString;
        private SqlConnection connection;

        public IBulkService ForTable(String tableName) {
            this.tableName = tableName;
            return this;
        }

        public IBulkService UseConnectionString(String connectionString) {
            if (connection != null)
                throw new InvalidOperationException("Connection is defined. Only one method will be used.");
            this.connectionString = connectionString;
            return this;
        }

        public IBulkService UseSqlConnection(SqlConnection connection) {
            if (!String.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("ConnectionString is used. Only one method will be used.");
            this.connection = connection;
            return this;
        }

        public void Insert<T>(IEnumerable<T> dataList) where T : class {
            if (dataList == null)
                throw new ArgumentNullException("dataList");
            if (!dataList.Any())
                throw new ArgumentOutOfRangeException("dataList");

            DataTable dt = ConvertToBulkDataTable(dataList);

            const SqlBulkCopyOptions Options = SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction | SqlBulkCopyOptions.CheckConstraints;
            var bulk = String.IsNullOrEmpty(connectionString) ?
                new SqlBulkCopy(connection, Options, connection.BeginTransaction()) : 
                new SqlBulkCopy(connectionString, Options);
            bulk.DestinationTableName = tableName;
            bulk.WriteToServer(dt);
        }

        private DataTable ConvertToBulkDataTable<T>(IEnumerable<T> dataList) where T : class {
            var dt = new DataTable();
            Type tType = typeof(T);
            PropertyInfo[] props = tType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props) {
                var attr = prop.GetCustomAttributes(typeof(BulkColumnAttribute), false).SingleOrDefault() as BulkColumnAttribute;
                dt.Columns.Add(attr == null ? prop.Name : attr.BulkColumnName);
            }
    
            foreach (T data in dataList) {
                PropertyInfo[] iprops = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                DataRow dr = dt.NewRow();
                foreach (DataColumn col in dt.Columns) {
                    String column = col.ColumnName;
                    PropertyInfo prop = iprops.Single(p => {var attr = p.GetCustomAttributes(typeof(BulkColumnAttribute), false).SingleOrDefault() as BulkColumnAttribute; return (attr == null ? p.Name : attr.BulkColumnName) == column; });
                    dr[column] = prop.GetValue(data, null);
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}