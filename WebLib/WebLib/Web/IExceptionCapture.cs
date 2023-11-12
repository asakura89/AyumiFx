using System;
using System.Web.UI;

namespace WebLib.Web
{
    public interface IExceptionCapture
    {
        void CaptureException(TemplateControl control, Exception ex);
    }
}