using BasicWebAPI.Models;

namespace BasicWebAPI.Interfaces
{
    public interface IContactService
    {
        ICollection<Contact> GetContacts();
        Contact GetContact(int id);
        int CreateContact(Contact contact);
        bool ContactExists(int id);
        
        Contact UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
        bool Save();
        ICollection<Contact> GetContactsWithCompanyAndCountry();
        ICollection<Contact> FilterContacts(int? countryId, int? companyId);

    }
}
