using WebLib.Data;
using WebLib.Extensions.Model;
using WebLib.Extensions.Model.Repository;
using WebLib.Extensions.Model.Service;
using WebLibTest.Services;

namespace WebLibTest.Factories
{
    public class WebLibTestServiceFactory : ISimpleServiceFactory
    {
        private readonly MSSQL sequel = new MSSQL(new MSSQL.DefaultMSSQLConfiguration());

        public IUserLoginPolicy CreateUserLoginPolicy()
        {
            return new DefaultUserLoginPolicy();
        }

        public LoginService CreateLoginService()
        {
            IUserRepository userRepo = null;
            ILoginInfoRepository loginInfoRepo = null;
            IUserLoginPolicy loginPolicy = null;
            ISimpleObjectFactory objectFactory = null;
            return new WebLibTestLoginService(userRepo, loginInfoRepo, loginPolicy, objectFactory);
        }

        public ModuleService CreateModuleService()
        {
            return new WebLibTestModuleService(sequel);
        }

        public GroupMemberService CreateGroupMemberService()
        {
            return new WebLibTestGroupMemberService(sequel);
        }

        public GroupAccessService CreateGroupAccessService()
        {
            return new WebLibTestGroupAccessService(sequel);
        }

        public UserService CreateUserService()
        {
            return new WebLibTestUserService(sequel);
        }

        public AppParameterService CreateAppParameterService()
        {
            return new WebLibTestAppParameterService(sequel);
        }

        public IErrorLogService CreateErrorLogService()
        {
            return new WebLibErrorLogService();
        }
    }
}