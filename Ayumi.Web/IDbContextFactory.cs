using Databossy;

//using ServiceStack.OrmLite;

namespace WebApp {
    public interface IDbContextFactory {
        //IDbConnectionFactory ConnectionFactory { get; }
        IDatabase Context { get; }
    }
}