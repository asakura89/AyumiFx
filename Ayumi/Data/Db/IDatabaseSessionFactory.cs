using System;
using System.Data.Common;

namespace Ayumi.Data.Db {
    public interface IDatabaseSessionFactory {
        IDatabase CreateSession(DbProviderFactory provider);
        IDatabase CreateSession(DbProviderFactory provider, String contextName);
    }
}