using BasicWebAPI.Data;
using BasicWebAPI.Interfaces;
using BasicWebAPI.Models;

namespace BasicWebAPI.Services
{
    public class CountryService : ICountryService
    {
        private readonly MyDbContext _context;
        public CountryService(MyDbContext context)
        {
            _context = context;
        }

        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.CountryId == id);
        }

        public int CreateCountry(Country country)
        {
            _context.Add(country);
            Save();
            return country.CountryId;

        }

        public void DeleteCountry(Country country)
        {
            _context.Remove(country);
            Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(p => p.CountryId).ToList();

        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(e => e.CountryId == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Country UpdateCountry(Country country)
        {
            var existingCountry = _context.Countries.FirstOrDefault(c => c.CountryId == country.CountryId);
            if (existingCountry != null)
            {
                _context.Entry(existingCountry).CurrentValues.SetValues(country);
                Save();
                return existingCountry;
            }
            else
            {
                throw new InvalidOperationException("Country not found");
            }
        }
    }
}
