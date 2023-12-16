using BasicWebAPI.Models;

namespace BasicWebAPI.Dto
{
    public class ContactDto
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }


        public int CompanyId { get; set; }
        //public CompanyDto Company { get; set; }


        public int CountryId { get; set; }
        //public CountryDto Country { get; set; }
    }
}
