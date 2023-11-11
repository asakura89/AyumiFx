using System;
using System.Text;

namespace Ayumi.Logger
{
    public static class LogExtensions
    {
        public const String Break = "{break}";

        public static String GetExceptionMessage(this Exception ex)
        {
            var errorList = new StringBuilder();
            if (ex.InnerException != null)
                errorList.AppendLine(GetExceptionMessage(ex.InnerException));
            errorList.AppendLine(ex.Message);
            errorList.AppendLine(ex.StackTrace);

            return errorList.ToString();
        }
    }
}