using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_GROUPRepository : RepoManager<ADM_R_GROUP>
    {
        public ADM_R_GROUP GetById(String groupId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_group", OperatorConstant.OP_EQUAL, groupId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_GROUP> groupList = GetDataList(conditionList);

            return groupList.Count == 0 ? null : groupList[0];
        }
    } 
}
