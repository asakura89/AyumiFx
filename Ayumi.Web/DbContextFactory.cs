using System;
using System.Configuration;
using Databossy;

//using ServiceStack.OrmLite;

namespace WebApp {
    public class DbContextFactory : IDbContextFactory {
        private readonly String contextName;

        public DbContextFactory(String contextName) {
            if (String.IsNullOrEmpty(contextName))
                throw new ArgumentNullException("contextName");

            this.contextName = contextName;
        }

        /*
        private IDbConnectionFactory connectionFactory;
        public IDbConnectionFactory ConnectionFactory
        {
            get
            {
                if (connectionFactory == null)
                    GetContext();
                return connectionFactory;
            }
        }
        */

        public IDatabase Context =>
            new DatabaseFactory()
                .CreateSession(
                    Convert.ToString(ConfigurationManager
                        .ConnectionStrings[contextName]), true);
    }
}