using System;
using System.Collections.Generic;
using WebLib.Constant;
using WebLib.Data;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Service
{
    public abstract class AppParameterService
    {
        private readonly MSSQL dbHandler;

        protected AppParameterService(MSSQL dbHandler)
        {
            if (dbHandler == null)
                throw new ArgumentNullException("dbHandler");

            this.dbHandler = dbHandler;
        }

        public IAppParameter GetById(String paramId)
        {
            List<IAppParameter> appParameterList = dbHandler.GetDataList<IAppParameter>(new Condition(Connector.And, IAppParameterProperty.ParamId, Operator.Equal, paramId), false);

            return CollectionExtension.FirstOrDefault(appParameterList);
        }

        public String GetParameterDesc(String paramId)
        {
            IAppParameter appParameter = GetById(paramId);

            return appParameter == null ? String.Empty : appParameter.ParamDesc;
        }
    }
}