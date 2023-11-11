using System;

namespace Ayumi.Extension {
    public static class DateTimeExt {
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

        static readonly String[] fullmonths = new[] {
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
        public static String CurrentSimpleReadableDate => DateTime.Now.AsSimpleReadableDate();
        public static String AsReadableDay(this DateTime date) => days[Convert.ToInt32(date.DayOfWeek) + 1];
        public static String AsReadableDate(this DateTime date) => $"{date.Day} {fullmonths[date.Month]} {date.Year}";
        public static String AsSimpleReadableDate(this DateTime date) => $"{date.Day} {months[date.Month]}";
        public static DateTime DefaultSqlDateTime { get; } = new DateTime(1753, 1, 1);
        public const String DefaultSqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public static Boolean IsMinSqlDateTime(this DateTime dateToConfirm) => dateToConfirm.CompareTo(DefaultSqlDateTime) == 0;
        public static String ToSqlCompatibleFormatString(this DateTime dateToFormat) => dateToFormat.ToString(DefaultSqlDateTimeFormat);
    }
}