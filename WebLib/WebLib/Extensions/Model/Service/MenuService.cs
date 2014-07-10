using System;
using WebLib.Data;

namespace WebLib.Extensions.Model.Service
{
    public abstract class MenuService
    {
        private readonly MSSQL dbHandler;

        protected MenuService(MSSQL dbHandler)
        {
            if (dbHandler == null)
                throw new ArgumentNullException("dbHandler");

            this.dbHandler = dbHandler;
        }
    }
}