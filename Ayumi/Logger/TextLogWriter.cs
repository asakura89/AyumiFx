using System;
using System.IO;
using System.Text;

namespace Ayumi.Logger
{
    public class TextLogWriter : ILogWriter
    {
        private readonly String logFilepath;
        public TextLogWriter(String logFilepath)
        {
            this.logFilepath = logFilepath;
        }

        public void Write(String formattedMessage)
        {
            String dirpath = logFilepath.Replace(Path.GetFileName(logFilepath), String.Empty);
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
            var writer = File.Exists(logFilepath) ? File.AppendText(logFilepath) : new StreamWriter(logFilepath, false, Encoding.UTF8);
            using (writer)
                if (formattedMessage.Contains(LogExtensions.Break))
                    writer.WriteLine();
                else
                    writer.WriteLine(formattedMessage);
        }
    }
}