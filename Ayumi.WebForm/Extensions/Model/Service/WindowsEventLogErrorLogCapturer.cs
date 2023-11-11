using System;
using System.Diagnostics;
using WebLib.Constant;

namespace WebLib.Extensions.Model.Service
{
    public class WindowsEventLogErrorLogCapturer : IErrorLogCapturer
    {
        public void CaptureException(Exception ex)
        {
            var errEventLog = new EventLog("Application", Environment.MachineName, App.Name);
            if (!EventLog.SourceExists(errEventLog.Source, errEventLog.MachineName))
                EventLog.CreateEventSource(errEventLog.Source, errEventLog.Log);

            errEventLog.WriteEntry(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
        }
    }
}