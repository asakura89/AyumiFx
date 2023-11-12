using System;
using System.Collections.Generic;
using WebLib.Data;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Repository
{
    public interface ILoginInfoRepository : IRepository<ILoginInfo>
    {
        IEnumerable<ILoginInfo> GetByUser(String userId);
        ILoginInfo GetByUserAndSession(String userId, String sessionId);
        IEnumerable<ILoginInfo> GetLoginInfoListInOtherDimension(String userId, String sessionId);
    }
}