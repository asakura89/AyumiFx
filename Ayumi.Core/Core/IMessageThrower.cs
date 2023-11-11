using System;

namespace Ayumi.Core
{
    public interface IMessageThrower
    {
        String WarningThrower { set; }
        String ExceptionThrower { set; }
        String InfoThrower { set; }
    }
}