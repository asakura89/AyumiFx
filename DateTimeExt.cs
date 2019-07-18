using System;

namespace Deto {
    public static class DateTimeExt {
        static String[] months = new[] {
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

        static String[] fullmonths = new[] {
            String.Empty,
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };

        static String[] days = new[] {
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
        public static String CurrentSimpleReadableDate => DateTime.Now.AsSimpleReadableDate();
        public static String AsReadableDay(this DateTime date) => days[Convert.ToInt32(date.DayOfWeek) + 1];
        public static String AsReadableDate(this DateTime date) => $"{date.Day} {fullmonths[date.Month]} {date.Year}";
        public static String AsSimpleReadableDate(this DateTime date) => $"{date.Day} {months[date.Month]}";
    }
}
