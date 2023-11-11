using WebLib.Data;
using WebLib.Extensions.Model.Service;

namespace WebLibTest.Services
{
    public class WebLibTestUserService : UserService
    {
        public WebLibTestUserService(MSSQL dbHandler) : base(dbHandler)
        {
        }
    }
}