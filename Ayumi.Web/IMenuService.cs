using System;
using System.Collections.Generic;

namespace WebApp {
    public interface IMenuService {
        IList<WebAppMenu> GetRootMenuList(String groupMemberId);
        WebAppMenu GetMenu(String moduleId);
    }
}