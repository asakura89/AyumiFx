using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_GROUP_MEMBERSRepository : RepoManager<ADM_R_GROUP_MEMBERS>
    {
        public ADM_R_GROUP_MEMBERS GetById(String groupMembersId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_group_members", OperatorConstant.OP_EQUAL, groupMembersId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_GROUP_MEMBERS> groupMemberList = GetDataList(conditionList);

            return groupMemberList.Count == 0 ? null : groupMemberList[0];
        }

        public List<ADM_R_GROUP_MEMBERS> GetByUser(String userId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_user", OperatorConstant.OP_EQUAL, userId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_GROUP_MEMBERS> groupMemberList = GetDataList(conditionList);

            return groupMemberList;
        }

        public ADM_R_GROUP_MEMBERS GetByUserAndGroup(String userId, String groupId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_user", OperatorConstant.OP_EQUAL, userId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_group", OperatorConstant.OP_EQUAL, groupId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_GROUP_MEMBERS> groupMemberList = GetDataList(conditionList);

            return groupMemberList.Count == 0 ? null : groupMemberList[0];
        }
    } 
}
