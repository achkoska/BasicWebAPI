using BasicWebAPI.Data;
using BasicWebAPI.Models;

namespace BasicWebAPI
{
    public class Seed
    {
        private readonly MyDbContext myDbContext;  

        public Seed(MyDbContext context) 
        {
            this.myDbContext = context;
        }

        public void SeedDataContext()
        {
            if (!myDbContext.Contacts.Any() && !myDbContext.Companies.Any() && !myDbContext.Countries.Any())
            {
                
                var countries = new List<Country>
                {
                    new Country { CountryName = "North Macedonia" },
                    new Country { CountryName = "Spain" },
                    new Country { CountryName = "France" },
                    new Country { CountryName = "Germany" },
                    
                };

                myDbContext.Countries.AddRange(countries);
                myDbContext.SaveChanges();


                var companies = new List<Company>
                {
                    new Company { CompanyName = "Aspekt" },
                    new Company { CompanyName = "Company2" },
                    new Company { CompanyName = "Company3" },
                    new Company { CompanyName = "Company4" },
                    
                };

                myDbContext.Companies.AddRange(companies);
                myDbContext.SaveChanges();

                var contacts = new List<Contact>
                {
                    new Contact { ContactName = "Evgenija", CompanyId = companies[0].CompanyId, CountryId = countries[0].CountryId },
                    new Contact { ContactName = "Ana", CompanyId = companies[1].CompanyId, CountryId = countries[1].CountryId }
                    
                };



                myDbContext.Contacts.AddRange(contacts);

                myDbContext.SaveChanges();
            }
        }
    }
}
