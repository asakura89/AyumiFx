using System;
using System.Collections.Generic;
using WebLib.Constant;
using WebLib.Data;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Service
{
    public abstract class ModuleService
    {
        private readonly MSSQL dbHandler;

        protected ModuleService(MSSQL dbHandler)
        {
            if (dbHandler == null)
                throw new ArgumentNullException("dbHandler");

            this.dbHandler = dbHandler;
        }

        public String GetByLink(String moduleLink)
        {
            var conditionList = new List<Condition>();
            conditionList.Add(new Condition(Connector.And, IModuleProperty.Link, Operator.Like, moduleLink));
            conditionList.Add(new Condition(Connector.And, IAuditTrailProperty.IsDeleted, Operator.Equal, Boolean.FalseString));
            List<IModule> moduleList = dbHandler.GetDataList<IModule>(conditionList, false);
            String moduleId = moduleList.Count == 0 ? String.Empty : moduleList[0].ModuleId;

            return moduleId;
        }

        public List<IModule> GetByParent(String parentId)
        {
            var conditionList = new List<Condition>();
            conditionList.Add(new Condition(Connector.And, IModuleProperty.ModuleParent, Operator.Like, parentId));
            conditionList.Add(new Condition(Connector.And, IAuditTrailProperty.IsDeleted, Operator.Equal, Boolean.FalseString));
            List<IModule> moduleList = dbHandler.GetDataList<IModule>(conditionList, false);

            return moduleList;
        }

        public List<IMenu> GetRootMenuList(String userId, String groupId)
        {
            /*String query =
                "SELECT DISTINCT * FROM VIEW_MENU WHERE ID_GROUP = @0 " +
                "AND ID_USER = @1 AND MODULE_PARENT IS NULL";*/

            var conditionList = new List<Condition>();
            conditionList.Add(new Condition(Connector.And, IMenuProperty.UserId, Operator.Equal, userId));
            conditionList.Add(new Condition(Connector.And, IMenuProperty.GroupId, Operator.Equal, groupId));
            conditionList.Add(new Condition(Connector.And, IMenuProperty.ParentId, Operator.Is, "NULL"));

            List<IMenu> rootMenuList = dbHandler.GetDataList<IMenu>(conditionList, false);

            return rootMenuList;
        }

        public IMenu GetMenu(String moduleId)
        {
            //String query = "SELECT DISTINCT * FROM VIEW_MENU WHERE ID_MODULE = @0";
            //query, false, moduleId
            List<IMenu> menuList = dbHandler.GetDataList<IMenu>(new Condition(Connector.And, IMenuProperty.ModuleId, Operator.Equal, moduleId), false);

            return CollectionExtended.FirstOrDefault(menuList);
        }
    }
}