using WebLib.Extensions.Model.Service;

namespace WebLib.Extensions.Model
{
    public interface ISimpleServiceFactory
    {
        IUserLoginPolicy CreateUserLoginPolicy();
        LoginService CreateLoginService();
        ModuleService CreateModuleService();
        GroupAccessService CreateGroupAccessService();
        IErrorLogService CreateErrorLogService();
    }
}