using System;

namespace Ayumi.Extensions
{
    public static class ExtendedDateTime
    {
        public static DateTime DefaultSqlDateTime = new DateTime(1753, 1, 1);
        public const String DefaultSqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static Boolean IsMinSqlDateTime(this DateTime dateToConfirm)
        {
            return dateToConfirm.CompareTo(DefaultSqlDateTime) == 0;
        }

        public static String ToSqlCompatibleFormatString(this DateTime dateToFormat)
        {
            return dateToFormat.ToString(DefaultSqlDateTimeFormat);
        }
    }
}