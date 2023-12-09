using System;

namespace Shiro {
    public interface ILogWriter {
        void Write(String formattedMessage);
    }
}