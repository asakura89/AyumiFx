using System;

namespace Ayumi.Logger
{
    public interface ILogMessageFormatter
    {
        String Format(String severity, String message);
    }
}