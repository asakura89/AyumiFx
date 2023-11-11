using System;

namespace WebLib.Data
{
    public interface IMSSQLConfiguration
    {
        String ConnectionString { get; set; }
        String ConnectionProvider { get; set; }
        MSSQL.ConnectionStringSource ConnectionStringSource { get; set; }
        MSSQL.ConnectionStringEncryptionType ConnectionStringEncryptionType { get; set; }
    }
}