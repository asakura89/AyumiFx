using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Databossy;
using WebApp.Config;
using WebApp.Model;

namespace WebApp {
    public class WebAppMenuService {
        const String WebAppsMenuId = "webapps.menu";

        readonly IDbContextFactory factory;
        readonly IMapper mapper;

        public WebAppMenuService(IDbContextFactory factory) {
            this.factory = factory;
            var config = new MapperConfiguration(conf => conf.CreateMap<m_Menu, WebAppMenu>());
            mapper = config.CreateMapper();
        }

        public void CacheMenu(WebAppSignedInInfo userInfo) {
            IList<WebAppMenu> menuList = GetAuthorizedMenu(userInfo);
            HttpContext.Current.Session.Add(WebAppsMenuId, menuList);
        }

        IList<WebAppMenu> GetAuthorizedMenu(WebAppSignedInInfo userInfo) {
            var menuList = new List<m_Menu>();
            using (IDatabase db = factory.Context) {
                String query = @"
                    SET NOCOUNT ON

                    DECLARE @@menu TABLE (
                        MenuId VARCHAR(50),
                        ActivityId VARCHAR(50),
                        MenuName VARCHAR(50),
                        Link VARCHAR(50),
                        ParentId VARCHAR(50),
                        [Priority] INT,
                        IsShown BIT,
                        Icon VARCHAR(50)
                    )

                    /*
                    IF (@IsAdmin = 1)
                    BEGIN
                        INSERT INTO @@menu
                        SELECT m.*
                        FROM m_Menu m
                        LEFT JOIN m_Authorization au ON m.ActivityId = au.ActivityId
                        GROUP BY m.MenuId, m.ActivityId, m.MenuName, m.Link, m.ParentId, m.[Priority], m.IsShown, m.Icon
                    END
                    ELSE
                    BEGIN
                        INSERT INTO @@menu
                        SELECT m.*
                        FROM m_Menu m
                        LEFT JOIN m_Authorization au ON m.ActivityId = au.ActivityId
                        WHERE au.GroupId = @GroupId AND au.Username = @Username
                        GROUP BY m.MenuId, m.ActivityId, m.MenuName, m.Link, m.ParentId, m.[Priority], m.IsShown, m.Icon
                    END
                    */

                    INSERT INTO @@menu
                    SELECT m.*
                    FROM m_Menu m
                    LEFT JOIN m_Authorization au ON m.ActivityId = au.ActivityId
                    WHERE @IsAdmin = 1 OR (au.GroupId = @GroupId AND au.Username = @Username)
                    GROUP BY m.MenuId, m.ActivityId, m.MenuName, m.Link, m.ParentId, m.[Priority], m.IsShown, m.Icon

                    SELECT * FROM (
                        SELECT * FROM m_Menu
                        WHERE ParentId IS NULL
                        AND MenuId IN (SELECT DISTINCT ParentId FROM @@menu)
                        UNION
                        SELECT * FROM @@menu
                    ) tmp
                    ORDER BY tmp.[Priority]

                    SET NOCOUNT OFF";

                foreach (String group in userInfo.GroupList) {
                    menuList.AddRange(
                        db.NQuery<m_Menu>(query,
                            new {
                                GroupId = group,
                                userInfo.Username,
                                IsAdmin = userInfo.GroupList.Contains(MainConfig.AdminGroupNameDnx) ? 1 : 0
                            })
                    );
                }
            }

            IList<WebAppMenu> distinct = menuList
                .Select(mapper.Map<m_Menu, WebAppMenu>)
                .GroupBy(m => m.MenuId)
                .Select(g => g.First())
                .OrderBy(m => m.Priority)
                .ToList();

            return distinct;
        }

        public IList<WebAppMenu> GetCachedMenu() {
            return HttpContext.Current.Session[WebAppsMenuId] as List<WebAppMenu> ?? new List<WebAppMenu>();
        }

        public IList<WebAppMenu> GetRootMenu() {
            return GetCachedMenu()
                .Where(m => String.IsNullOrEmpty(m.ParentId) && m.IsShown)
                .ToList();
        }

        public IList<WebAppMenu> GetChildMenu(WebAppMenu menu) {
            return GetCachedMenu()
                .Where(m => !String.IsNullOrEmpty(m.ParentId) && m.ParentId == menu.MenuId && m.IsShown)
                .ToList();
        }

        public Boolean IsHasChild(WebAppMenu menu) {
            return GetCachedMenu()
                .Any(m => m.ParentId == menu.MenuId && m.IsShown);
        }
    }
}