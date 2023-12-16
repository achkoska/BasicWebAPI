using BasicWebAPI.Models;

namespace BasicWebAPI.Interfaces
{
    public interface ICountryService
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        int CreateCountry(Country country);
        bool CountryExists(int id);
        Country UpdateCountry(Country country);
        void DeleteCountry(Country country);
        bool Save();
    }
}
