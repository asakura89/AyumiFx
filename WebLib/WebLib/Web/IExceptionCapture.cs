using System;
using System.Web.UI;

namespace WebLib.Web
{
    public interface IExceptionCapture
    {
        void OnException(TemplateControl control, Exception ex);
    }
}