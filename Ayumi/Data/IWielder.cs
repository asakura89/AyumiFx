using System;
using System.Collections.Generic;

namespace Ayumi.Data {
    public interface IWielder {
        IWielder AddCounter(Int32 currentCounter, Int32 valueLength);
        IWielder AddCounter(Int32 currentCounter, Int32 increment, Int32 valueLength);
        IWielder AddCounter(Int32 currentCounter, Int32 increment, Int32 valueLength, String backSeparator);
        IWielder AddCounter(Int32 currentCounter, Int32 valueLength, String backSeparator);
        IWielder AddDate();
        IWielder AddDate(Int32 valueLength, String backSeparator);
        IWielder AddDate(String backSeparator);
        IWielder AddGUIDString();
        IWielder AddGUIDString(String backSeparator);
        IWielder AddLongDay();
        IWielder AddLongDay(IList<String> customDayList);
        IWielder AddLongDay(IList<String> customDayList, String backSeparator);
        IWielder AddLongDay(String backSeparator);
        IWielder AddLongMonth();
        IWielder AddLongMonth(IList<String> customMonthList);
        IWielder AddLongMonth(IList<String> customMonthList, String backSeparator);
        IWielder AddLongMonth(String backSeparator);
        IWielder AddLongYear();
        IWielder AddLongYear(String backSeparator);
        IWielder AddNumericDay();
        IWielder AddNumericDay(String backSeparator);
        IWielder AddNumericMonth();
        IWielder AddNumericMonth(String backSeparator);
        IWielder AddRandomAlphaNumeric(Int32 valueLength);
        IWielder AddRandomAlphaNumeric(Int32 valueLength, Boolean uppercase);
        IWielder AddRandomAlphaNumeric(Int32 valueLength, Boolean uppercase, String backSeparator);
        IWielder AddRandomNumber(Int32 valueLength);
        IWielder AddRandomNumber(Int32 valueLength, String backSeparator);
        IWielder AddRandomString(Int32 valueLength);
        IWielder AddRandomString(Int32 valueLength, AlphaType type);
        IWielder AddRandomString(Int32 valueLength, AlphaType type, String backSeparator);
        IWielder AddShortDay();
        IWielder AddShortDay(IList<String> customDayList);
        IWielder AddShortDay(IList<String> customDayList, String backSeparator);
        IWielder AddShortDay(String backSeparator);
        IWielder AddShortMonth();
        IWielder AddShortMonth(IList<String> customMonthList);
        IWielder AddShortMonth(IList<String> customMonthList, String backSeparator);
        IWielder AddShortMonth(String backSeparator);
        IWielder AddShortYear();
        IWielder AddShortYear(String backSeparator);
        IWielder AddString(String value, Int32 valueLength);
        IWielder AddString(String value, Int32 valueLength, String backSeparator);
        String BuildKey();
    }
}