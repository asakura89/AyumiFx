using System;
using System.Collections.Generic;
using WebLib.Constant;
using WebLib.Data;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Service
{
    public abstract class UserService
    {
        private readonly MSSQL dbHandler;

        protected UserService(MSSQL dbHandler)
        {
            if (dbHandler == null)
                throw new ArgumentNullException("dbHandler");

            this.dbHandler = dbHandler;
        }

        public IUser GetByNIK(String nik)
        {
            var conditionList = new List<Condition>();
            conditionList.Add(new Condition(Connector.And, IUserProperty.NIK, Operator.Equal, nik));
            conditionList.Add(new Condition(Connector.And, IAuditTrailProperty.IsDeleted, Operator.Equal, Boolean.FalseString));
            List<IUser> userList = dbHandler.GetDataList<IUser>(conditionList, false);

            return CollectionExtension.FirstOrDefault(userList);
        }

        public IUser GetByUserId(String userId)
        {
            var conditionList = new List<Condition>();
            conditionList.Add(new Condition(Connector.And, IUserProperty.Username, Operator.Equal, userId));
            conditionList.Add(new Condition(Connector.And, IAuditTrailProperty.IsDeleted, Operator.Equal, Boolean.FalseString));
            List<IUser> userList = dbHandler.GetDataList<IUser>(conditionList, false);

            return CollectionExtension.FirstOrDefault(userList);
        }
    }
}