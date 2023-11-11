using System;

namespace WebLib.Extensions.Model.Object
{
    public interface IModule
    {
        String ModuleId { get; set; }
        String ModuleName { get; set; }
        String ModuleParent { get; set; }
        String Description { get; set; }
        String Link { get; set; }
        Int32 Priority { get; set; }
        //String LinkTarget { get; set; }
        //Int32 Icon { get; set; }
        //Boolean Popup { get; set; }
    }
}