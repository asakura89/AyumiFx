using System;

namespace WebLib.Extensions.Model.Object
{
    public interface IErrorInfo
    {
        String UserId { get; set; }
        String ModuleId { get; set; }
        DateTime ErrorTime { get; set; }
        String ErrorMessage { get; set; }
    }
}