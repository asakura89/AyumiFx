using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class ADM_R_MODULERepository : RepoManager<ADM_R_MODULE>
    {
        public ADM_R_MODULE GetById(String moduleId)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "id_module", OperatorConstant.OP_EQUAL, moduleId));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_MODULE> moduleList = GetDataList(conditionList);

            return moduleList.Count == 0 ? null : moduleList[0];
        }

        public String GetParentModuleId(String moduleId)
        {
            ADM_R_MODULE module = GetById(moduleId);
            return module == null ? String.Empty : module.module_parent;
        }

        public List<ADM_R_MODULE> GetModuleListByParent(String parentId)
        {
            String query = String.Format("SELECT * FROM ADM_R_MODULE WHERE MODULE_PARENT {0} AND DELETED = 0",
                String.IsNullOrEmpty(parentId) ? "IS NULL" : "= '" + parentId + "'");
            List<ADM_R_MODULE> moduleList = GetDataList(query);

            return moduleList;
        }

        public String GetByLink(String link)
        {
            List<ConditionData> conditionList = new List<ConditionData>();
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "link", OperatorConstant.OP_LIKE, link));
            conditionList.Add(new ConditionData(ConnectorConstant.CON_AND, "deleted", OperatorConstant.OP_EQUAL, (false).ToString()));
            List<ADM_R_MODULE> moduleList = GetDataList(conditionList);
            ADM_R_MODULE firstModule = moduleList.Count == 0 ? new ADM_R_MODULE() : moduleList[0];

            return firstModule.id_module;
        }
    } 
}
