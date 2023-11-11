using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_LOG_LOGINRepository : RepoManager<ADM_R_LOG_LOGIN>
    {
        public ADM_R_LOG_LOGINRepository() : base(false) { }

        public ADM_R_LOG_LOGIN GetLoggedUserInfoList(String userId, String sessionId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_user", OperatorConstant.OP_EQUAL, userId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_session", OperatorConstant.OP_EQUAL, sessionId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "logout_time", OperatorConstant.OP_EQUAL, CommonFunction.ToSQLCompatibleFormat(CommonFunction.GetMinSQLDatetime())));
            List<ADM_R_LOG_LOGIN> logList = GetDataList(conditionList);

            return logList.Count == 0 ? null : logList[0];
        }

        public List<ADM_R_LOG_LOGIN> GetLoggedUserInfoList(String userId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_user", OperatorConstant.OP_EQUAL, userId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "logout_time", OperatorConstant.OP_EQUAL, CommonFunction.ToSQLCompatibleFormat(CommonFunction.GetMinSQLDatetime())));
            List<ADM_R_LOG_LOGIN> logList = GetDataList(conditionList);

            return logList;
        }
    }
}