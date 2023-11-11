using WebLib.Data;
using WebLib.Extensions.Model.Service;

namespace WebLibTest.Services
{
    public class WebLibTestGroupMemberService : GroupMemberService
    {
        public WebLibTestGroupMemberService(MSSQL dbHandler) : base(dbHandler)
        {
        }
    }
}