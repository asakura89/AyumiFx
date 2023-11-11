using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestInterface
{
    public interface IEmployee
    {
        EmployeeData GetEmployee(string employeeId);
        void SaveEmployee(EmployeeData employeeData);
        void DeleteEmployee(string employeeId);
    }
}
