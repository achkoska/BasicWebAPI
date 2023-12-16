using BasicWebAPI.Models;

namespace BasicWebAPI.Interfaces
{
    public interface ICompanyService
    {
        ICollection<Company> GetCompanies();
        Company GetCompany(int id);
        bool CompanyExists(int id);
        int CreateCompany(Company company);
        Company UpdateCompany(Company company);
        void DeleteCompany(Company company);
        bool Save();
    }

    

}
