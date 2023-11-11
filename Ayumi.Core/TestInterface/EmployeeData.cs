using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WK.DBUtility;

namespace TestInterface
{
    [Serializable]
    [WkTable("WK_Employee")]
    public class EmployeeData
    {
        [WkField(DbFieldType.PRIMARY_KEY)]
        public string EmployeeId { get; set; }

        [WkField]
        public string Name { get; set; }

        [WkItemField]
        public List<PhoneNumberData> PhoneNumbers { get; set; }

        public EmployeeData(string vEmployeeId)
            : this()
        {
            EmployeeId = vEmployeeId;
        }

        public EmployeeData()
        {
            EmployeeId = "";
            Name = "";
            PhoneNumbers = new List<PhoneNumberData>();
        }

        [Serializable]
        [WkTable("WK_EmployeePhoneNumber")]
        public class PhoneNumberData
        {
            [WkField(DbFieldType.PRIMARY_KEY)]
            public string PhoneNumber { get; set; }
        }
    }

   
}
