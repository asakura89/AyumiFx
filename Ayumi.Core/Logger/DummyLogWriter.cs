using System;

namespace Ayumi.Logger
{
    public class DummyLogWriter : ILogWriter
    {
        public void Write(String formattedMessage) { }
    }
}