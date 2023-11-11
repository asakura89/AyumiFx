using WebLib.Data;
using WebLib.Extensions.Model.Service;

namespace WebLibTest.Services
{
    public class WebLibTestModuleService : ModuleService
    {
        public WebLibTestModuleService(MSSQL dbHandler) : base(dbHandler)
        {
        }
    }
}