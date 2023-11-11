using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Ayumi.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ayumi.Test
{
    [TestClass]
    public class KeywielderTest
    {
        private String firstKey;
        private String secondKey;
        private String thirdKey;

        [TestInitialize]
        public void Initialize()
        {
            firstKey = Keywielder
                .New()
                .AddString("GRE", 3)
                .AddLongYear()
                .AddNumericMonth()
                .AddDate()
                .AddCounter(12, 4)
                .BuildKey();

            Debug.WriteLine(firstKey);

            secondKey = Keywielder
                .New()
                .AddString("GRE", 3, "/")
                .AddLongYear("-")
                .AddNumericMonth("-")
                .AddDate("/")
                .AddCounter(237, 5)
                .BuildKey();

            Debug.WriteLine(secondKey);

            thirdKey = Keywielder
                .New()
                .AddString("ASAKURA", 7)
                .AddCounter(77, 12, 2)
                .BuildKey();

            Debug.WriteLine(thirdKey);
        }

        [TestMethod]
        public void BuildKeyTest()
        {
            DateTime currentDate = DateTime.Now;
            String firstExpectation = currentDate.ToString("yyyyMMdd");
            String secondExpectation = currentDate.ToString("yyyy-MM-dd");

            Assert.AreEqual("GRE" + firstExpectation + "0013", firstKey); // change expectation to today's date
            Assert.AreEqual("GRE/" + secondExpectation + "/00238", secondKey); // change expectation to today's date
            Assert.AreEqual("ASAKURA89", thirdKey);

            Assert.AreNotEqual(firstKey, secondKey);
            Assert.AreNotEqual(secondKey, thirdKey);
            Assert.AreNotEqual(firstKey, thirdKey);
        }

        [TestMethod]
        public void AllOutKeyTest()
        {
            var defaultMonthArray = new[] { String.Empty, "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var indonesianMonthArray = new[] { String.Empty, "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            var indonesianMonthList = new List<String>(indonesianMonthArray.Select(month => month.ToUpperInvariant()));
            var indonesianDayArray = new[] { String.Empty, "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            var indonesianDayList = new List<String>(indonesianDayArray.Select(day => day.ToUpperInvariant()));

            DateTime currentDate = DateTime.Now;
            Int32 currentDayOfWeek = Convert.ToInt32(currentDate.DayOfWeek) + 1;
            Int32 currentMonth = currentDate.Month;
            String currentMonthString = indonesianMonthArray[currentMonth];
            String currentYearString = currentDate.Year.ToString();
            String currentDayString = indonesianDayArray[currentDayOfWeek];
            String separator = "•";
            String fourthKey = Keywielder
                .New()
                .AddString("DOC", 3)
                .AddString("UMENTATION", 5, "/")
                .AddShortYear("/")
                .AddLongYear("/")
                .AddShortMonth(separator)
                .AddShortMonth(indonesianMonthArray, separator)
                .AddLongMonth(separator)
                .AddLongMonth(indonesianMonthList, separator)
                .AddNumericMonth("/")
                .AddDate("/")
                .AddShortDay(separator)
                .AddLongDay(separator)
                .AddShortDay(indonesianDayArray, separator)
                .AddLongDay(indonesianDayList, separator)
                .AddNumericDay("/")
                .AddCounter(123, 400, 5)
                .BuildKey();

            Debug.WriteLine(fourthKey);

            Assert.AreEqual(
                "DOCUMENT/" + currentYearString.Substring(2, 2) + "/" + currentYearString + "/" + currentMonthString.ToUpper().Substring(0, 3) +
                separator + currentMonthString.Substring(0, 3) + separator +  defaultMonthArray[currentMonth].ToUpper() + separator +
                currentMonthString.ToUpper() + separator + currentMonth.ToString().PadLeft(2, '0') + "/" + currentDate.Day.ToString().PadLeft(2, '0') + "/" +
                currentDate.DayOfWeek.ToString().ToUpper().Substring(0, 3) + separator + currentDate.DayOfWeek.ToString().ToUpper() + separator +
                currentDayString.Substring(0, 3) + separator + currentDayString.ToUpper() + separator + currentDayOfWeek.ToString().PadLeft(2, '0') +
                "/00523",
                fourthKey);
        }

        [TestMethod]
        public void RandomKeyTest()
        {
            String fifthKey = Keywielder.New().AddRandomString(8).BuildKey();
            Debug.WriteLine(fifthKey);

            Assert.IsTrue(fifthKey.Length == 8);
            Assert.IsTrue(Regex.IsMatch(fifthKey, @"^[a-zA-Z]+$"));

            String sixthKey = Keywielder.New().AddRandomAlphaNumeric(12).BuildKey();
            Debug.WriteLine(sixthKey);

            Assert.IsTrue(sixthKey.Length == 12);
            Assert.IsTrue(Regex.IsMatch(sixthKey, @"^[a-zA-Z0-9]+$"));

            String seventhKey = Keywielder.New().AddRandomNumber(6).BuildKey();
            Debug.WriteLine(seventhKey);

            Assert.IsTrue(seventhKey.Length == 6);
            Assert.IsTrue(Regex.IsMatch(seventhKey, @"^[0-9]+$"));

            Assert.AreNotEqual(fifthKey, sixthKey);
            Assert.AreNotEqual(fifthKey, seventhKey);
            Assert.AreNotEqual(sixthKey, seventhKey);
        }

        [TestMethod]
        public void GuidKeyTest()
        {
            String guidKey = Keywielder.New().AddGUIDString().BuildKey();
            Debug.WriteLine(guidKey);

            Assert.IsTrue(guidKey.Length == 32);
        }
    }
}
