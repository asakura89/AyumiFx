using WebLib.Extensions.Model.Service;

namespace WebLib.Extensions.Model
{
    public interface ISimpleServiceFactory
    {
        IUserLoginPolicy CreateUserLoginPolicy();
        LoginService CreateLoginService();
        ModuleService CreateModuleService();
        GroupMemberService CreateGroupMemberService();
        GroupAccessService CreateGroupAccessService();
        UserService CreateUserService();
        AppParameterService CreateAppParameterService();
        IErrorLogService CreateErrorLogService();
    }
}