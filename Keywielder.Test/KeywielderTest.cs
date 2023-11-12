using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace Keywielder.Test {
    public class KeywielderTest : IDisposable {
        String firstKey;
        String secondKey;
        String thirdKey;

        public KeywielderTest() {
            firstKey = Wielder
                .New()
                .AddString("GRE", 3)
                .AddLongYear()
                .AddNumericMonth()
                .AddDate()
                .AddLeftPadded(w => w.AddCounter(12), 4, '0')
                .BuildKey();

            Debug.WriteLine(firstKey);

            secondKey = Wielder
                .New()
                .AddString("GRE", 3)
                .AddString("/")
                .AddLongYear()
                .AddString("-")
                .AddNumericMonth()
                .AddString("-")
                .AddDate()
                .AddString("/")
                .AddLeftPadded(w => w.AddCounter(237), 5, '0')
                .BuildKey();

            Debug.WriteLine(secondKey);

            thirdKey = Wielder
                .New()
                .AddString("ASAKURA", 7)
                .AddCounter(77, 12)
                .BuildKey();

            Debug.WriteLine(thirdKey);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing) =>
            firstKey = secondKey = thirdKey = null;

        [Fact]
        public void BuildKeyTest() {
            DateTime currentDate = DateTime.Now;
            String firstExpectation = currentDate.ToString("yyyyMMdd");
            String secondExpectation = currentDate.ToString("yyyy-MM-dd");

            Assert.Equal("GRE" + firstExpectation + "0013", firstKey); // change expectation to today's date
            Assert.Equal("GRE/" + secondExpectation + "/00238", secondKey); // change expectation to today's date
            Assert.Equal("ASAKURA89", thirdKey);

            Assert.NotEqual(firstKey, secondKey);
            Assert.NotEqual(secondKey, thirdKey);
            Assert.NotEqual(firstKey, thirdKey);
        }

        [Fact]
        public void AddRandomStringTest() {
            var list = new List<String>();
            Int32 i = 0;
            Int32 maxi = 5 ^ 5;
            for (; i < maxi; i++)
                list.Add(Wielder.New().AddRandomString(5).BuildKey());

            Assert.False(list.ContainsDuplicate());
        }

        [Fact]
        public void AddRandomNumberTest() {
            var list = new List<String>();
            Int32 i = 0;
            Int32 maxi = 5 ^ 5;
            for (; i < maxi; i++)
                list.Add(Wielder.New().AddRandomNumber(5).BuildKey());

            Assert.False(list.ContainsDuplicate());
        }

        [Fact]
        public void AddRandomAlphaNumericTest() {
            var list = new List<String>();
            Int32 i = 0;
            Int32 maxi = 5 ^ 5;
            for (; i < maxi; i++)
                list.Add(Wielder.New().AddRandomAlphaNumeric(5).BuildKey());

            Assert.False(list.ContainsDuplicate());
        }

        [Fact]
        public void AddStringTest() {

        }

        [Fact]
        public void AllOutKeyTest() {
            String[] defaultMonthArray = new[] { String.Empty, "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            String[] indonesianMonthArray = new[] { String.Empty, "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            var indonesianMonthList = new List<String>(indonesianMonthArray.Select(month => month.ToUpperInvariant()));
            String[] indonesianDayArray = new[] { String.Empty, "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            var indonesianDayList = new List<String>(indonesianDayArray.Select(day => day.ToUpperInvariant()));

            DateTime currentDate = DateTime.Now;
            Int32 currentDayOfWeek = Convert.ToInt32(currentDate.DayOfWeek) + 1;
            Int32 currentMonth = currentDate.Month;
            String currentMonthString = indonesianMonthArray[currentMonth];
            String currentYearString = currentDate.Year.ToString();
            String currentDayString = indonesianDayArray[currentDayOfWeek];
            String separator = "•";
            String fourthKey = Wielder
                .New()
                .AddString("DOC", 3)
                .AddString("UMENTATION", 5)
                .AddString("/")
                .AddShortYear()
                .AddString("/")
                .AddLongYear()
                .AddString("/")
                .AddShortMonth(indonesianMonthList)
                .AddString(separator)
                .AddShortMonth(indonesianMonthArray)
                .AddString(separator)
                .AddLongMonth()
                .AddString(separator)
                .AddLongMonth(indonesianMonthList)
                .AddString(separator)
                .AddNumericMonth()
                .AddString("/")
                .AddDate()
                .AddString("/")
                .AddShortDay()
                .AddString(separator)
                .AddLongDay()
                .AddString(separator)
                .AddShortDay(indonesianDayArray)
                .AddString(separator)
                .AddLongDay(indonesianDayList)
                .AddString(separator)
                .AddNumericDay()
                .AddString("/")
                .AddLeftPadded(w => w.AddCounter(123, 400), 5, '0')
                .BuildKey();

            Debug.WriteLine(fourthKey);

            Assert.Equal(
                "DOCUMENT/" + currentYearString.Substring(2, 2) + "/" + currentYearString + "/" + currentMonthString.ToUpper().Substring(0, 3) +
                separator + currentMonthString.Substring(0, 3) + separator +  defaultMonthArray[currentMonth].ToUpper() + separator +
                currentMonthString.ToUpper() + separator + currentMonth.ToString().PadLeft(2, '0') + "/" + currentDate.Day.ToString().PadLeft(2, '0') + "/" +
                currentDate.DayOfWeek.ToString().ToUpper().Substring(0, 3) + separator + currentDate.DayOfWeek.ToString().ToUpper() + separator +
                currentDayString.Substring(0, 3) + separator + currentDayString.ToUpper() + separator + currentDayOfWeek.ToString().PadLeft(2, '0') +
                "/00523",
                fourthKey);
        }

        [Fact]
        public void RandomKeyTest() {
            String fifthKey = Wielder.New().AddRandomString(8).BuildKey();
            Debug.WriteLine(fifthKey);

            Assert.True(fifthKey.Length == 8);
            Assert.Matches("^[a-zA-Z]+$", fifthKey);

            String sixthKey = Wielder.New().AddRandomAlphaNumeric(12).BuildKey();
            Debug.WriteLine(sixthKey);

            Assert.True(sixthKey.Length == 12);
            Assert.Matches("^[a-zA-Z0-9]+$", sixthKey);

            String seventhKey = Wielder.New().AddRandomNumber(6).BuildKey();
            Debug.WriteLine(seventhKey);

            Assert.True(seventhKey.Length == 6);
            Assert.Matches("^[0-9]+$", seventhKey);

            Assert.NotEqual(fifthKey, sixthKey);
            Assert.NotEqual(fifthKey, seventhKey);
            Assert.NotEqual(sixthKey, seventhKey);
        }

        [Fact]
        public void GuidKeyTest() {
            String guidKey = Wielder.New().AddGuidString().BuildKey();
            Debug.WriteLine(guidKey);

            Assert.True(guidKey.Length == 32);
        }
    }
}
