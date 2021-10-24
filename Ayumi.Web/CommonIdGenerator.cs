using System;
using System.Globalization;

namespace WebApp {
    public static class CommonIdGenerator {
        public static Int32 GetBaseOnDate(DateTime date) =>
            Convert.ToInt32(date.ToString(CommonFormat.ReversedDate, CultureInfo.InvariantCulture));

        public static Int32 GetBaseOnDate() => GetBaseOnDate(DateTime.Now);
    }
}