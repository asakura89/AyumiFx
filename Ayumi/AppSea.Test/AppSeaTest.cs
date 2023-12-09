using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AppSea.Test
{
    [TestClass]
    public class AppSeaTest
    {
        [TestMethod]
        public void GetTest()
        {
            var config = AppConfig.Get<BusinessSettings>();
            Assert.IsNotNull(config);
            Assert.IsTrue(config.IsShowingColumnPangkat);
            Assert.IsTrue(config.IsShowingColumnGolongan);
            Assert.IsTrue(config.IsShowingColumnRuang);
        }

        [TestMethod]
        public void GetTest2()
        {
            var config = AppConfig.Get<MachineDataFormatConfiguration>();
            Assert.IsTrue(String.IsNullOrEmpty(config.AdditionalAttendanceDataFormatNamespace));
            try
            {
                if (String.IsNullOrEmpty(config.AdditionalAttendanceDataFormatNamespace))
                    throw new BadConfigurationException("AdditionalAttendanceDataFormatNamespace");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.ToLowerInvariant().Contains("not configured"));
            }
        }

        [TestMethod]
        public void GetTest3()
        {
            var config = AppConfig.Get<LoadDocumentConfiguration>();
            Assert.IsFalse(String.IsNullOrEmpty(config.DocumentMapJson));

            var expected = new List<DocumentMap>
            {
                new DocumentMap
                {
                    Pattern = "pay%",
                    MapsToCollectionCode = "MERCU_PAYROLL"
                },
                new DocumentMap
                {
                    Pattern = "%thr%",
                    MapsToCollectionCode = "MERCU_THR"
                },
                new DocumentMap
                {
                    Pattern = "%lembur%",
                    MapsToCollectionCode = "MERCU_OVERTIME"
                },
                new DocumentMap
                {
                    Pattern = "%prodi%",
                    MapsToCollectionCode = "MERCU_BONUS_STUDYPROGRAM"
                },
                new DocumentMap
                {
                    Pattern = "%bonus%",
                    MapsToCollectionCode = "MERCU_BONUS"
                }
            };

            var actual = JsonConvert.DeserializeObject<List<DocumentMap>>(config.DocumentMapJson);

            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[2].Pattern, actual[2].Pattern);
        }

        [TestMethod]
        public void GetBySectionTest()
        {
            var section1 = AppConfig.GetBySection<ConfigSection>();
            var section2 = AppConfig.GetBySection<ConfigSection2>();

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);

            Assert.IsFalse(String.IsNullOrEmpty(section1.AdditionalExcelReportNamespace));
            Assert.IsFalse(String.IsNullOrEmpty(section2.AdditionalExcelReportNamespace));

            Assert.IsTrue(section1.AdditionalExcelReportNamespace == "Enta.Module.ExcelReport.Additional");
            Assert.IsTrue(section2.AdditionalExcelReportNamespace == "Enta.Module.ExcelReport.Additional");

            Assert.IsTrue(String.IsNullOrEmpty(section1.AdditionalAttendanceDataFormatNamespace));
            Assert.IsTrue(String.IsNullOrEmpty(section2.AdditionalAttendanceDataFormatNamespace));
        }
    }
}
