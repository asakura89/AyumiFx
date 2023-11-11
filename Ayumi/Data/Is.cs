using System;

namespace Ayumi.Data
{
    public class Is
    {
        private Is() { }
        public static Is It() { return new Is(); }

        public Is Null(Object obj, String customErrorMessage = "Object is null.")
        {
            if (obj == null)
                throw new InvalidOperationException(customErrorMessage);
            return this;
        }

        public Is Empty(String str, String customErrorMessage = "String is empty.")
        {
            if (str == String.Empty)
                throw new InvalidOperationException(customErrorMessage);
            return this;
        }
    }
}