using System;
using System.Collections.Generic;

namespace WebApp {
    public interface IUserService : IRepository<WebAppUser> {
        void UpdateBadSignInCounter(String username);
        IList<WebAppUser> GetAllNonAdmin(String tenant, String adminGroupName);
    }
}