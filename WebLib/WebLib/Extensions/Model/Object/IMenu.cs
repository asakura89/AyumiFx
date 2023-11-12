using System;

namespace WebLib.Extensions.Model.Object
{
    public interface IMenu
    {
        String UserId { get; set; }
        String GroupId { get; set; }
        String ModuleId { get; set; }
        String ModuleName { get; set; }
        String Link { get; set; }
        String ParentId { get; set; }
        String SubModule { get; set; }
    }
}