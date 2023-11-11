using System;

namespace Ayumi.Logger
{
    public class DefaultLogFormatter : ILogMessageFormatter
    {
        public String Format(String severity, String message)
        {
            String formattedSeverity = severity.Replace("{", String.Empty).Replace("}", String.Empty);
            return
                DateTime.Now.ToString("dd.MM.yyyy-HH.mm.ss") +
                "/" + formattedSeverity + "/" + message;
        }
    }
}