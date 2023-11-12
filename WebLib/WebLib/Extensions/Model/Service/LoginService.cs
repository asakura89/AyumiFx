using System;
using System.Collections.Generic;
using System.Security.Authentication;
using WebLib.Constant;
using WebLib.Extensions.Model.Object;
using WebLib.Extensions.Model.Repository;

namespace WebLib.Extensions.Model.Service
{
    public abstract class LoginService
    {
        private readonly IUserRepository userRepo;
        private readonly ILoginInfoRepository loginInfoRepo;
        private readonly IUserLoginPolicy loginPolicy;
        private readonly ISimpleObjectFactory objectFactory;

        protected LoginService(IUserRepository userRepo, ILoginInfoRepository loginInfoRepo, IUserLoginPolicy loginPolicy, ISimpleObjectFactory objectFactory)
        {
            if (userRepo == null)
                throw new ArgumentNullException("userRepo");
            if (loginInfoRepo == null)
                throw new ArgumentNullException("loginInfoRepo");
            if (loginPolicy == null)
                throw new ArgumentNullException("loginPolicy");
            if (objectFactory == null)
                throw new ArgumentNullException("objectFactory");

            this.userRepo = userRepo;
            this.loginInfoRepo = loginInfoRepo;
            this.loginPolicy = loginPolicy;
            this.objectFactory = objectFactory;
        }

        public void Login(String userId, String passwordString, String sessionId)
        {
            IUser currentUser = userRepo.GetById(userId);
            try
            {
                if (currentUser == null)
                    throw new AuthenticationException(CommonException.CantLogin);

                if (loginPolicy.IsUserTouchCounterLimit(currentUser))
                    throw new AuthenticationException(CommonException.ContactAdmin);

                if (IsLoggedIn(currentUser.UserId, sessionId))
                    KickUserInOtherDimension(currentUser.UserId, sessionId);
            }
            catch
            {
                if (currentUser != null)
                    CaptureFailedLogin(currentUser);
                throw;
            }
        }

        public Boolean IsLoggedIn(String userId, String sessionId)
        {
            ILoginInfo loginInfo = loginInfoRepo.GetByUserAndSession(userId, sessionId);
            return loginInfo != null;
        }

        private void KickUserInOtherDimension(String userId, String sessionId)
        {
            IEnumerable<ILoginInfo> loginInfoList = loginInfoRepo.GetLoginInfoListInOtherDimension(userId, sessionId);
            foreach (ILoginInfo loginInfo in loginInfoList)
            {
                loginInfo.LogoutTime = DateTime.Now;
                loginInfoRepo.Update(loginInfo);
            }
        }

        private void CaptureFailedLogin(IUser currentUser)
        {
            currentUser.Counter += 1;
            userRepo.Update(currentUser);
        }

        public void Logout(String userId, String sessionId)
        {
            ILoginInfo currentLoginInfo = loginInfoRepo.GetByUserAndSession(userId, sessionId);
            currentLoginInfo.LogoutTime = DateTime.Now;
            loginInfoRepo.Update(currentLoginInfo);
        }

        public void CaptureLoginInfo(String userId, String userIpAddress, String sessionId)
        {
            var loginInfo = objectFactory.CreateLoginInfo();
            loginInfo.LogId = Guid.NewGuid().ToString("N");
            loginInfo.UserId = userId;
            loginInfo.IpAddress = userIpAddress;
            loginInfo.SessionId = sessionId;
            loginInfo.LoginTime = DateTime.Now;
            loginInfo.LogoutTime = CommonFunction.GetMinSQLDatetime();

            loginInfoRepo.Insert(loginInfo);
        }
    }
}