using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_USERRepository : RepoManager<ADM_R_USER>
    {
        public ADM_R_USER GetById(String userId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "user_id", OperatorConstant.OP_EQUAL, userId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_USER> userList = GetDataList(conditionList);

            return userList.Count == 0 ? null : userList[0];
        }

        public ADM_R_USER GetByNIK(String nik)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "nik", OperatorConstant.OP_EQUAL, nik));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_USER> userList = GetDataList(conditionList);

            return userList.Count == 0 ? null : userList[0];
        }

        public ADM_R_USER GetByUsername(String username)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "username", OperatorConstant.OP_EQUAL, username));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_USER> userList = GetDataList(conditionList);

            return userList.Count == 0 ? null : userList[0];
        }
    }
}