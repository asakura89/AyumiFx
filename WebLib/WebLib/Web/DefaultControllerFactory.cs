using System;
using System.Collections.Generic;

namespace WebLib.Web
{
    public abstract class DefaultControllerFactory : IControllerFactory
    {
        private readonly Dictionary<String, BaseController> controllerMap = new Dictionary<String, BaseController>();
        public virtual BaseController CreateController(String controllerName)
        {
            if (controllerMap.ContainsKey(controllerName))
                return controllerMap[controllerName];

            return null;
        }
    }
}