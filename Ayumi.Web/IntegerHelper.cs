using System;

namespace WebApp {
    public static class IntegerHelper {
        public static String AsZeroedInteger(this Int32 integer) => integer.ToString().PadLeft(2, '0');
    }
}