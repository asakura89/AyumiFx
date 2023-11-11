using System.Collections.Generic;
using System.Linq;
using TestInterface;
using WK.AppUtility;
using WK.DBUtility;

namespace TestManager
{
    public sealed class CompanyManager : WkManagerBase, ICompany
    {
        CompanyData ICompany.GetCompany(string companyId)
        {
            using (DbAccess dbManager = GetDbAccess("Company.GetCompany()"))
            {
                using (var sqlCommand = dbManager.CreateCommand())
                    return WkEntitySql.SelectById(sqlCommand, new CompanyData(companyId));
            }
        }

        void ICompany.InsertCompany(CompanyData companyData)
        {
            using (DbAccess dbManager = GetDbAccess("Company.InsertCompany()"))
            {
                dbManager.BeginTransaction();
                using (var sqlCommand = dbManager.CreateCommand())
                    WkEntitySql.Insert(sqlCommand, companyData);
                dbManager.CommitTransaction();
            }
        }

        void ICompany.UpdateCompany(CompanyData companyData)
        {
            using (DbAccess dbManager = GetDbAccess("Company.UpdateCompany()"))
            {
                dbManager.BeginTransaction();
                using (var sqlCommand = dbManager.CreateCommand())
                    WkEntitySql.Update(sqlCommand, companyData);
                dbManager.CommitTransaction();
            }
        }

        void ICompany.DeleteCompany(string companyId)
        {
            using (DbAccess dbManager = GetDbAccess("Company.DeleteCompany()"))
            {
                dbManager.BeginTransaction();
                using (var sqlCommand = dbManager.CreateCommand())
                    WkEntitySql.Delete(sqlCommand, new CompanyData(companyId));
                dbManager.CommitTransaction();
            }
        }

        List<CompanyData> ICompany.SelectCompany(string whereClause)
        {
            using (DbAccess dbManager = GetDbAccess("Company.SelectCompany()"))
            {
                using (var sqlCommand = dbManager.CreateCommand())
                    return WkEntitySql.SelectWithWhereClause<CompanyData>(sqlCommand, whereClause).ToList();
            }
        }
    }
}
