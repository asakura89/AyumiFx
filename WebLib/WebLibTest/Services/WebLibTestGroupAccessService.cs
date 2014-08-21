using System;
using WebLib.Data;
using WebLib.Extensions.Model.Service;

namespace WebLibTest.Services
{
    public class WebLibTestGroupAccessService : GroupAccessService
    {
        public WebLibTestGroupAccessService(MSSQL dbHandler) : base(dbHandler)
        {
        }

        public override bool IsAuthorized(String userId, String moduleId)
        {
            throw new System.NotImplementedException();
        }
    }
}