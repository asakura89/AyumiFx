using System;

namespace Shiro {
    public class ConsoleLogWriter : ILogWriter {
        public void Write(String formattedMessage) {
            if (formattedMessage.Contains(LogExtensions.Break))
                Console.WriteLine();
            else
                Console.WriteLine(formattedMessage);
        }
    }
}