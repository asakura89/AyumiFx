using System;

namespace Shiro {
    public interface ILogMessageFormatter {
        String Format(String severity, String message);
    }
}