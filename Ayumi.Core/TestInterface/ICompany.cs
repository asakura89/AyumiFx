using System.Collections.Generic;

namespace TestInterface
{
    public interface ICompany
    {
        CompanyData GetCompany(string companyId);
        void InsertCompany(CompanyData companyData);
        void UpdateCompany(CompanyData companyData);
        void DeleteCompany(string companyId);
        List<CompanyData> SelectCompany(string whereClause);
    }
}
