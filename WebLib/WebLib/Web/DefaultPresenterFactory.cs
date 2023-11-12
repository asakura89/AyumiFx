using System;
using System.Collections.Generic;

namespace WebLib.Web
{
    public abstract class DefaultPresenterFactory : IPresenterFactory
    {
        private readonly Dictionary<String, BasePresenter> controllerMap = new Dictionary<String, BasePresenter>();
        public virtual BasePresenter CreateController(String controllerName)
        {
            if (controllerMap.ContainsKey(controllerName))
                return controllerMap[controllerName];

            return null;
        }
    }
}