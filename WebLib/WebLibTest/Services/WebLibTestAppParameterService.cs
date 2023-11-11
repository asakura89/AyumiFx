using WebLib.Data;
using WebLib.Extensions.Model.Service;

namespace WebLibTest.Services
{
    public class WebLibTestAppParameterService : AppParameterService
    {
        public WebLibTestAppParameterService(MSSQL dbHandler) : base(dbHandler)
        {
        }
    }
}