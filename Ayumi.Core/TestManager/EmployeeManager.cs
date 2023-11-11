using System;
using TestInterface;
using WK.AppUtility;
using WK.DBUtility;

namespace TestManager
{
    public sealed class EmployeeManager : WkManagerBase, IEmployee
    {
        #region IEmployee Members

        EmployeeData IEmployee.GetEmployee(string employeeId)
        {
            using (DbAccess dbAccess = GetDbAccess("Employee.GetEmployee()"))
            {
                using (var sqlCommand = dbAccess.CreateCommand())
                {
                    EmployeeData data = WkEntitySql.SelectById(sqlCommand, new EmployeeData(employeeId));
                    if (data == null)
                        throw new Exception(string.Format("Employee {0} does not exist.", employeeId));
                    return data;
                }
            }
        }

        void IEmployee.SaveEmployee(EmployeeData employeeData)
        {
            using (DbAccess dbMan = GetDbAccess("Employee.SaveEmployee()"))
            {
                dbMan.BeginTransaction();
                using (var sqlCommand = dbMan.CreateCommand())
                    WkEntitySql.Save(sqlCommand, employeeData);
                dbMan.CommitTransaction();
            }
        }

        void IEmployee.DeleteEmployee(string employeeId)
        {
            using (DbAccess dbMan = GetDbAccess("Employee.DeleteEmployee()"))
            {
                dbMan.BeginTransaction();
                using (var sqlCommand = dbMan.CreateCommand())
                    WkEntitySql.Delete(sqlCommand, new EmployeeData(employeeId));
                dbMan.CommitTransaction();
            }
        }

        #endregion
    }
}
