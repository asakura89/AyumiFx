using System;
using System.Collections.Generic;
using WebLib.Data;

namespace WebLib.Extensions.Model
{
    public class MainMenuRepository : RepoManager<MainMenu>
    {
        public List<MainMenu> GetRootMenuList(String userId, String groupId)
        {
            String query =
                "SELECT DISTINCT * FROM VIEW_MENU WHERE ID_GROUP = @0 " +
                "AND ID_USER = @1 AND MODULE_PARENT IS NULL";
            List<MainMenu> rootMenuList = GetDataList(query, groupId, userId);

            return rootMenuList;
        }

        public MainMenu GetMenu(String moduleId)
        {
            String query = "SELECT DISTINCT * FROM VIEW_MENU WHERE ID_MODULE = @0";
            List<MainMenu> menuList = GetDataList(query, moduleId);

            return menuList.Count == 0 ? null : menuList[0];
        }
    }
}
