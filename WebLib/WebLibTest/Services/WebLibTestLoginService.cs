using WebLib.Extensions.Model;
using WebLib.Extensions.Model.Repository;
using WebLib.Extensions.Model.Service;

namespace WebLibTest.Services
{
    public class WebLibTestLoginService : LoginService
    {
        public WebLibTestLoginService(IUserRepository userRepo, ILoginInfoRepository loginInfoRepo, IUserLoginPolicy loginPolicy, ISimpleObjectFactory objectFactory) : base(userRepo, loginInfoRepo, loginPolicy, objectFactory)
        {
        }
    }
}