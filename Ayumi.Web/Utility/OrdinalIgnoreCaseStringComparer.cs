using System;
using System.Collections.Generic;

namespace WebApp.Utility {
    public class OrdinalIgnoreCaseStringComparer : IEqualityComparer<String> {
        public Boolean Equals(String x, String y) =>
            (String.IsNullOrEmpty(x) ? String.Empty : x)
            .Equals(y, StringComparison.OrdinalIgnoreCase);

        public Int32 GetHashCode(String obj) => obj.GetHashCode();
    }
}