using System;

namespace Shiro {
    public class DummyLogWriter : ILogWriter {
        public void Write(String formattedMessage) { }
    }
}