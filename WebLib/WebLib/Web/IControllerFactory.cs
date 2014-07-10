using System;

namespace WebLib.Web
{
    public interface IControllerFactory
    {
        BaseController CreateController(String controllerName);
    }
}