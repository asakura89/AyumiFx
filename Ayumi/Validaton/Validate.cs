using System;

namespace Ayumi.Validaton {
    public static class Validate {
        public static void Null(Object obj, String customErrorMessage = "Object is null.") {
            if (obj == null)
                throw new InvalidOperationException(customErrorMessage);
        }

        public static void Empty(String str, String customErrorMessage = "String is empty.") {
            if (str == String.Empty)
                throw new InvalidOperationException(customErrorMessage);
        }
    }
}