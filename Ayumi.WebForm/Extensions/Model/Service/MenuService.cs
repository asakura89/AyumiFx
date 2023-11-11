using System;
using System.Collections.Generic;
using WebLib.Constant;
using WebLib.Data;
using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model.Service
{
    public abstract class MenuService
    {
        private readonly MSSQL dbHandler;

        protected MenuService(MSSQL dbHandler)
        {
            if (dbHandler == null)
                throw new ArgumentNullException("dbHandler");

            this.dbHandler = dbHandler;
        }

        public IEnumerable<IMenu> GetRootMenuList(String groupId)
        {
            var conditionList = new List<Condition>();
            conditionList.Add(new Condition(Connector.And, IMenuProperty.GroupId, Operator.Equal, groupId));
            conditionList.Add(new Condition(Connector.And, IMenuProperty.ParentId, Operator.Is, "NULL"));
            conditionList.Add(new Condition(Connector.And, IAuditTrailProperty.IsDeleted, Operator.Equal, Boolean.FalseString));
            List<IMenu> rootMenuList = dbHandler.GetDataList<IMenu>(conditionList, false);

            return rootMenuList;
        }

        public IMenu GetMenu(String moduleId)
        {
            List<IMenu> menuList = dbHandler.GetDataList<IMenu>(new Condition(Connector.And, IMenuProperty.ModuleId, Operator.Equal, moduleId), false);

            return CollectionExtension.FirstOrDefault(menuList);
        }
    }
}