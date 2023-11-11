using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_GROUP_ACCESSRepository : RepoManager<ADM_R_GROUP_ACCESS>
    {
        public ADM_R_GROUP_ACCESS GetById(String groupAccessId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_group_access", OperatorConstant.OP_EQUAL, groupAccessId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_GROUP_ACCESS> groupAccessList = GetDataList(conditionList);

            return groupAccessList.Count == 0 ? null : groupAccessList[0];
        }

        public ADM_R_GROUP_ACCESS GetByGroupAndAttibute(String groupId, String aId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_group", OperatorConstant.OP_EQUAL, groupId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "aid", OperatorConstant.OP_EQUAL, aId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_GROUP_ACCESS> groupAccessList = GetDataList(conditionList);

            return groupAccessList.Count == 0 ? null : groupAccessList[0];
        }

        // TODO: needs VIEW_ADM_R_GROUP_ACCESS
        public Boolean IsAuthorized(String userId, String moduleId)
        {
            String query =
                "SELECT DISTINCT COUNT(0) FROM VIEW_ADM_R_GROUP_ACCESS WHERE ID_USER = @0 " +
                "AND (AID LIKE '%OPEN%' OR AID LIKE '%VIEW%') AND ID_MODULE = @1";
            Int32 moduleCount = ExecScalar<Int32>(query, userId, moduleId);

            return moduleCount > 0;
        }
    } 
}
