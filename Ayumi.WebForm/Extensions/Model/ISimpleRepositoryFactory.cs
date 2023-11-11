using WebLib.Extensions.Model.Repository;

namespace WebLib.Extensions.Model
{
    public interface ISimpleRepositoryFactory
    {
        IErrorInfoRepository CreateErrorInfoRepository();
        ILoginInfoRepository CreateLoginInfoRepository();
        IUserRepository CreateUserRepository();
    }
}