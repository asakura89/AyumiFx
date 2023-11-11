using WebLib.Extensions.Model.Object;

namespace WebLib.Extensions.Model
{
    public interface ISimpleObjectFactory
    {
        IAppParameter CreateAppParameter();
        IErrorInfo CreateErrorInfo();
        ILoginInfo CreateLoginInfo();
        IUser CreateUser();
    }
}
