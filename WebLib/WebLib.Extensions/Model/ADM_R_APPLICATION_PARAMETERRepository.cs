using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_APPLICATION_PARAMETERRepository : RepoManager<ADM_R_APPLICATION_PARAMETER>
    {
        public ADM_R_APPLICATION_PARAMETER GetById(String paramId)
        {
            ConditionData condition = new ConditionData(ConnectorConstant.CON_AND, "id_param", OperatorConstant.OP_EQUAL, paramId);
            List<ADM_R_APPLICATION_PARAMETER> appParamList = GetDataList(condition);

            return appParamList.Count == 0 ? null : appParamList[0];
        }

        public String GetAppName()
        {
            return GetApplicationParameter("AppName");
        }

        public String GetFooter()
        {
            return GetApplicationParameter("AppFooter");
        }

        public String GetApplicationParameter(String parameterName)
        {
            ADM_R_APPLICATION_PARAMETER appParam = GetById(parameterName);
            return appParam == null ? String.Empty : appParam.param_desc;
        }
    }
}
