using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Ayumi.Data.Db {
    public interface IDatabase : IDisposable {
        String ContextName { get; }
        String ConnectionString { get; }
        DbProviderFactory Provider { get; }
        DataTable QueryDataTable(String queryString);
        DataTable QueryDataTable(String queryString, params Object[] queryParams);
        DataTable NQueryDataTable(String queryString, Object paramObj);
        DataSet QueryDataSet(String queryString);
        DataSet QueryDataSet(String queryString, params Object[] queryParams);
        DataSet NQueryDataSet(String queryString, Object paramObj);
        IEnumerable<T> Query<T>(String queryString);
        IEnumerable<T> Query<T>(String queryString, params Object[] queryParams);
        IEnumerable<T> NQuery<T>(String queryString, Object paramObj);
        T QuerySingle<T>(String queryString);
        T QuerySingle<T>(String queryString, params Object[] queryParams);
        T NQuerySingle<T>(String queryString, Object paramObj);
        T QueryScalar<T>(String queryString);
        T QueryScalar<T>(String queryString, params Object[] queryParams);
        T NQueryScalar<T>(String queryString, Object paramObj);
        Int32 Execute(String queryString);
        Int32 Execute(String queryString, params Object[] queryParams);
        Int32 NExecute(String queryString, Object paramObj);
    }
}