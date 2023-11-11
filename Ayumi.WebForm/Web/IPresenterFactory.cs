using System;

namespace WebLib.Web
{
    public interface IPresenterFactory
    {
        BasePresenter CreateController(String controllerName);
    }
}