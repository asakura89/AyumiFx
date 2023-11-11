using System;
using System.Data.Common;
using Ayumi.Configuration;

namespace Ayumi.Data.Db {
    public class DatabaseSessionFactory : IDatabaseSessionFactory {
        public IDatabase CreateSession(DbProviderFactory provider) => CreateSession(provider, ConfigurationManager.ConnectionStrings[0].Key);
        public IDatabase CreateSession(DbProviderFactory provider, String contextName) => new Database(provider, contextName);
    }
}