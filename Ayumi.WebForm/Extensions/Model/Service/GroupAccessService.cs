using System;
using WebLib.Data;

namespace WebLib.Extensions.Model.Service
{
    public abstract class GroupAccessService
    {
        private readonly MSSQL dbHandler;

        protected GroupAccessService(MSSQL dbHandler)
        {
            if (dbHandler == null)
                throw new ArgumentNullException("dbHandler");

            this.dbHandler = dbHandler;
        }

        public abstract Boolean IsAuthorized(String userId, String moduleId);
    }
}