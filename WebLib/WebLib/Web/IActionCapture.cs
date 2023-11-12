using System;

namespace WebLib.Web
{
    public interface IActionCapture
    {
        void CaptureAction(String action, String actionDesc);
    }
}
