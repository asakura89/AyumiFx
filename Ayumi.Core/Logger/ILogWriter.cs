using System;

namespace Ayumi.Logger
{
    public interface ILogWriter
    {
        void Write(String formattedMessage);
    }
}