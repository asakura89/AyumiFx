using System;
using System.Globalization;

namespace WebApp {
    public static class DateTimeHelper {
        static readonly String[] months = new[] {
            String.Empty,
            "Jan",
            "Feb",
            "March",
            "Apr",
            "May",
            "June",
            "July",
            "August",
            "Sept",
            "Oct",
            "Nov",
            "Dec"
        };

        static readonly String[] days = new[] {
            String.Empty,
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"
        };

        public static String CurrentReadableDay => DateTime.Now.AsReadableDay();
        public static String CurrentReadableDate => DateTime.Now.AsReadableDate();
        public static String AsReadableDay(this DateTime date) => days[Convert.ToInt32(date.DayOfWeek) + 1];
        public static String AsReadableDate(this DateTime date) => $"{date.Day} {months[date.Month]}";

        public static DateTime AsBackendDate(this String uiDate) => DateTime.ParseExact(uiDate, CommonFormat.UIDate, CultureInfo.InvariantCulture);
    }
}