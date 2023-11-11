using System;
using WK.DBUtility;

namespace TestInterface
{
    [Serializable]
    [WkTable("WK_Company")]
    public class CompanyData
    {
        [WkField("szCompanyId", DbFieldType.PRIMARY_KEY)]
        public string CompanyId { get; set; }

        [WkField("szName")]
        public string Name { get; set; }

        [WkField("szAddress")]
        public string Address { get; set; }

        [WkField("bIsActive")]
        public bool IsActive { get; set; }

        [WkField("dtmActiveDate")]
        public DateTime ActiveDate { get; set; }

        public CompanyData(string vCompanyId, string vName, string vAddress, bool vIsActive, DateTime vActiveDate)
        {
            CompanyId = vCompanyId;
            Name = vName;
            Address = vAddress;
            IsActive = vIsActive;
            ActiveDate = vActiveDate;
        }

        public CompanyData(string vCompanyId)
            : this(vCompanyId, "", "", false, DateTime.Now)
        {
        }

        public CompanyData()
            : this("")
        {
        }
    }
}
