using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_MODULE_ATTRIBUTRepository : RepoManager<ADM_R_MODULE_ATTRIBUT>
    {
        public List<ADM_R_MODULE_ATTRIBUT> GetAttributeListByModule(String moduleId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_module", OperatorConstant.OP_EQUAL, moduleId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_MODULE_ATTRIBUT> moduleAttrList = GetDataList(conditionList);

            return moduleAttrList;
        }

        public ADM_R_MODULE_ATTRIBUT GetByModuleAndAttribute(String moduleId, String aid)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_module", OperatorConstant.OP_EQUAL, moduleId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "aid", OperatorConstant.OP_EQUAL, aid));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_MODULE_ATTRIBUT> moduleAttrList = GetDataList(conditionList);

            return moduleAttrList.Count == 0 ? null : moduleAttrList[0];
        }

        public ADM_R_MODULE_ATTRIBUT GetByAttribute(String aid)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "aid", OperatorConstant.OP_EQUAL, aid));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_MODULE_ATTRIBUT> moduleAttrList = GetDataList(conditionList);

            return moduleAttrList.Count == 0 ? null : moduleAttrList[0];
        }
    } 
}
