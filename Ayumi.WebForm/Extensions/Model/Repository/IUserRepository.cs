using System;
using WebLib.Data;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Repository
{
    public interface IUserRepository : IRepository<IUser>
    {
        IUser GetByNIK(String userNIK);
    }
}