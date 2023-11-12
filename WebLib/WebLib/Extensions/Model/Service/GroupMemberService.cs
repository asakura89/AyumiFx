using System;
using System.Collections.Generic;
using WebLib.Constant;
using WebLib.Data;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Service
{
    public abstract class GroupMemberService
    {
        private readonly MSSQL dbHandler;

        protected GroupMemberService(MSSQL dbHandler)
        {
            if (dbHandler == null)
                throw new ArgumentNullException("dbHandler");

            this.dbHandler = dbHandler;
        }

        public List<IGroupMember> GetByUser(String userId)
        {
            var conditionList = new List<Condition>();
            conditionList.Add(new Condition(Connector.And, IGroupMemberProperty.UserId, Operator.Equal, userId));
            conditionList.Add(new Condition(Connector.And, IAuditTrailProperty.IsDeleted, Operator.Equal, Boolean.FalseString));
            List<IGroupMember> memberList = dbHandler.GetDataList<IGroupMember>(conditionList, false);

            return memberList;
        }

        public List<IGroupMember> GetByGroup(String groupId)
        {
            var conditionList = new List<Condition>();
            conditionList.Add(new Condition(Connector.And, IGroupMemberProperty.GroupId, Operator.Equal, groupId));
            conditionList.Add(new Condition(Connector.And, IAuditTrailProperty.IsDeleted, Operator.Equal, Boolean.FalseString));
            List<IGroupMember> memberList = dbHandler.GetDataList<IGroupMember>(conditionList, false);

            return memberList;
        }
    }
}