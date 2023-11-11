using System;

namespace WebLib.Extensions.Model.Object
{
    public interface ILoginInfo
    {
        String LogId { get; set; }
        String UserId { get; set; }
        String SessionId { get; set; }
        String IpAddress { get; set; }
        DateTime LoginTime { get; set; }
        DateTime LogoutTime { get; set; }
        //String Extension { get; set; }
        //String Description { get; set; }
    }
}