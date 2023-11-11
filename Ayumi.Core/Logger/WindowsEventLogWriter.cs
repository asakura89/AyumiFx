using System;
using System.Diagnostics;

namespace Ayumi.Logger
{
    public class WindowsEventLogWriter : ILogWriter
    {
        private readonly EventLog log;
        public WindowsEventLogWriter(String appName)
        {
            String machineName = Environment.MachineName;
            const String DefaultLogName = "Application";

            if (!EventLog.SourceExists(appName, machineName))
                EventLog.CreateEventSource(appName, DefaultLogName);
            log = new EventLog(DefaultLogName, machineName, appName);
        }

        public void Write(String formattedMessage)
        {
            if (!formattedMessage.Contains(LogExtensions.Break))
                log.WriteEntry(formattedMessage, formattedMessage.Contains(LogSeverity.Error)
                    ? EventLogEntryType.Error
                    : EventLogEntryType.Information);
        }
    }
}