using BasicWebAPI.Data;
using BasicWebAPI.Interfaces;
using BasicWebAPI.Models;

namespace BasicWebAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly MyDbContext _context;
        public CompanyService(MyDbContext context)
        {
            _context = context;
        }


        public ICollection<Company> GetCompanies()
        {
            return _context.Companies.OrderBy(p => p.CompanyId).ToList();

        }
        public int CreateCompany(Company company)
        {
            _context.Add(company);
            Save();
            return company.CompanyId;

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Company UpdateCompany(Company company)
        {
            var existingCompany = _context.Companies.FirstOrDefault(c => c.CompanyId == company.CompanyId);
            if (existingCompany != null)
            {
                _context.Entry(existingCompany).CurrentValues.SetValues(company);
                Save();
                return existingCompany;
            }
            else
            {
                throw new InvalidOperationException("Company not found");
            }
        }

        public bool CompanyExists(int id)
        {
            return _context.Companies.Any(c => c.CompanyId == id);
        }

        public void DeleteCompany(Company company)
        {
            _context.Remove(company);
            Save();
        }

        public Company GetCompany(int id)
        {
            return _context.Companies.Where(e => e.CompanyId == id).FirstOrDefault();
        }
    }
}
