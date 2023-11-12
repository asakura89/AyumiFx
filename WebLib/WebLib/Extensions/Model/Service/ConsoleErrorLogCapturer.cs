using System;
using System.Diagnostics;

namespace WebLib.Extensions.Model.Service
{
    public class ConsoleErrorLogCapturer : IErrorLogCapturer
    {
        public void CaptureException(Exception ex)
        {
            Debug.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
        }
    }
}