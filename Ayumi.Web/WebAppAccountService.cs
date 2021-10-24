using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Web;
using Arvy;
using AutoMapper;
using Databossy;
using WebApp.Config;
using WebApp.Model;
using WebApp.Utility;
using WebApp.ViewModel;

namespace WebApp {
    public class WebAppAccountService : IAccountService {
        readonly IDbContextFactory factory;
        readonly IMapper mapper;
        readonly IGroupMemberService memberSvc;
        readonly IUserSignInPolicy signInPolicy;
        readonly IUserService userSvc;

        public WebAppAccountService(IDbContextFactory factory) {
            this.factory = factory;
            userSvc = new WebAppUserService(factory);
            signInPolicy = new WebAppUserSignInPolicy();
            memberSvc = new WebAppMemberService(factory);
            var config = new MapperConfiguration(conf => {
                conf.CreateMap<m_SignInInfo, WebAppSignedInInfo>();
                conf.CreateMap<WebAppUser, m_User>();
            });
            mapper = config.CreateMapper();
        }

        public void SignIn(String username, String password) {
            if (IsAuthenticated())
                return;

            Boolean
                isUserExist = false,
                isSignInInfoValid = false;

            try {
                WebAppUser currentUser = userSvc.Get(MainConfig.Section.Tenant, username);
                isUserExist = currentUser != null;
                isSignInInfoValid = isUserExist && currentUser.Password.Equals(AccountExt.PasswordEnx(password),
                                        StringComparison.InvariantCulture);
                if (!isUserExist || !isSignInInfoValid)
                    throw new AuthenticationException(CommonException.CantLogin);

                if (signInPolicy.HasUserReachCounterLimit(currentUser))
                    throw new AuthenticationException(CommonException.ContactAdmin);

                if (IsSignedInInOtherDimension(currentUser.Username))
                    KickUserInOtherDimension(currentUser.Username);

                var signinInfo = new m_SignInInfo {
                    Username = currentUser.Username,
                    SignInTime = DateTime.Now,
                    SignOutTime = null,
                    IpAddress = HttpContext.Current.Request.UserHostAddress,
                    SessionId = HttpContext.Current.Session.SessionID
                };

                m_UserProfile profile = null;
                using (IDatabase db = factory.Context) {
                    db.NExecute(new StringBuilder()
                        .AppendLine("INSERT INTO m_SignInInfo")
                        .AppendLine("VALUES(@Username, @SessionId, @IpAddress, @SignInTime, @SignOutTime)")
                        .ToString(), signinInfo);

                    profile = db.NQuerySingle<m_UserProfile>(new StringBuilder()
                        .AppendLine("SELECT TOP 1 * FROM dbo.m_UserProfile")
                        .AppendLine("WHERE Username = @Username")
                        .ToString(), new {signinInfo.Username});
                }

                IList<String> groupList = memberSvc.GetByUser(username).Select(g => g.GroupId).ToList();
                var userInfo = new WebAppSignedInInfo {
                    Username = signinInfo.Username,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    GroupList = groupList,
                    SignInTime = signinInfo.SignInTime,
                    IpAddress = signinInfo.IpAddress,
                    SessionId = signinInfo.SessionId
                };
                WebAppCookie.CreateCookie(
                    String.Join(Delimiter.List.ToString(),
                        userInfo.Username,
                        userInfo.FirstName,
                        userInfo.LastName,
                        String.Join(Delimiter.Item.ToString(), groupList),
                        userInfo.SignInTime.ToString("ddMMyyyyhhmmss"),
                        userInfo.IpAddress,
                        userInfo.SessionId
                    )
                );
                HttpContext.Current.Session.Add("webapps.session", userInfo);
            }
            catch {
                if (isUserExist && !isSignInInfoValid)
                    userSvc.UpdateBadSignInCounter(username);
                throw;
            }
        }

        public Boolean IsAuthenticated() {
            /*return WebAppsCookie.IsCookieExists() ||
                (WebAppsCookie.IsCookieExists() && !WebAppsCookie.IsCookieExpired());*/
            return HttpContext.Current.Session["webapps.session"] != null;
        }

        public Boolean IsAuthorized(String activityId) {
            var loginSvc = new WebAppAccountService(factory);
            WebAppSignedInInfo userInfo = loginSvc.GetCachedUserInfo();
            if (userInfo.GroupList.Contains(MainConfig.AdminGroupNameDnx))
                return true;

            var menuSvc = new WebAppMenuService(factory);
            IList<WebAppMenu> menuList = menuSvc.GetCachedMenu();
            return menuList.Any(m => m.ActivityId == activityId);
        }

        public void SignOut() {
            WebAppSignedInInfo userInfo = GetCachedUserInfo();
            KickUserInOtherDimension(userInfo.Username);
            WebAppCookie.RemoveCookie();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public void ChangePassword(String tenant, WebAppChangePassword changePasswordInfo) {
            WebAppUser currentUser = userSvc.Get(MainConfig.Section.Tenant, changePasswordInfo.Username);
            if (currentUser == null)
                throw new InvalidOperationException(CommonException.InvalidUser);

            Boolean validPassword = currentUser.Password.Equals(
                AccountExt.PasswordEnx(changePasswordInfo.CurrentPassword), StringComparison.InvariantCulture);
            if (!validPassword)
                throw new InvalidOperationException(CommonException.BadPassword);

            changePasswordInfo.NewPassword = AccountExt.PasswordEnx(changePasswordInfo.NewPassword);
            using (IDatabase db = factory.Context) {
                String result = db.NQuerySingle<String>(this.LoadEmbeddedEqx("ChangePassword"), changePasswordInfo);
                result.AsActionResponseViewModel();
            }
        }

        Boolean IsSignedInInOtherDimension(String username) {
            IList<WebAppSignedInInfo> logInfoList = GetSignInInfoListInOtherDimension(username);
            return logInfoList.Any();
        }

        IList<WebAppSignedInInfo> GetSignInInfoListInOtherDimension(String username) {
            return GetSignInInfoListInOtherDimensionPure(username)
                .Select(mapper.Map<m_SignInInfo, WebAppSignedInInfo>)
                .ToList();
        }

        IList<m_SignInInfo> GetSignInInfoListInOtherDimensionPure(String username) {
            using (IDatabase db = factory.Context)
                return db.Query<m_SignInInfo>(
                        this.LoadEmbeddedEqx("GetSignInInfoListInOtherDimensionPure"),
                        username)
                    .ToList();
        }

        void KickUserInOtherDimension(String username) {
            using (IDatabase db = factory.Context) {
                IList<m_SignInInfo> infos = GetSignInInfoListInOtherDimensionPure(username);
                if (infos.Any()) {
                    /*
                    foreach (m_SignInInfo info in infos)
                        info.SignOutTime = DateTime.Now;
                    db.UpdateAll(infos);
                    */
                    db.WithTransaction(idb => {
                        foreach (m_SignInInfo info in infos) {
                            info.SignOutTime = DateTime.Now;
                            idb.Execute(
                                "UPDATE m_SignInInfo SET SignOutTime = @0 WHERE SessionId = @1 AND IpAddress = @2 AND Username = @3",
                                DateTime.Now, info.SessionId, info.IpAddress, info.Username);
                        }

                        return 0;
                    });
                }
            }
        }

        // NOTE: can't set and get cookie on same request --> http://stackoverflow.com/a/6810953/1181782
        public WebAppSignedInInfo GetCachedUserInfo() {
            String userInfo = WebAppCookie.GetUserInfo();
            if (!String.IsNullOrEmpty(userInfo)) {
                String[] splittedInfo = userInfo.Split(Delimiter.List);
                return new WebAppSignedInInfo {
                    Username = splittedInfo[0],
                    FirstName = splittedInfo[1],
                    LastName = splittedInfo[2],
                    GroupList = splittedInfo[3].Split(Delimiter.Item).ToList(),
                    SignInTime = DateTime.ParseExact(splittedInfo[4], "ddMMyyyyhhmmss", CultureInfo.InvariantCulture),
                    IpAddress = splittedInfo[5],
                    SessionId = splittedInfo[6]
                };
            }

            return HttpContext.Current.Session["webapps.session"] as WebAppSignedInInfo ?? new WebAppSignedInInfo();
        }
    }
}