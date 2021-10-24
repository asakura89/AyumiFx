using System.Collections.Generic;

namespace WebApp {
    public interface IActivityService : IRepository<WebAppActivity> {
        IList<WebAppActivity> GetAllFromAssembly();
    }
}