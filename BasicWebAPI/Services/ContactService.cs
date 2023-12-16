using BasicWebAPI.Data;
using BasicWebAPI.Interfaces;
using BasicWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicWebAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly MyDbContext _context;
        public ContactService(MyDbContext context)
        {
            _context = context;
        }

        public bool ContactExists(int id)
        {
            return _context.Contacts.Any(c => c.ContactId == id);
        }

        public int CreateContact(Contact contact)
        {
            _context.Add(contact);
            Save();
            return contact.ContactId;
        }

        public void DeleteContact(Contact contact)
        {
            _context.Remove(contact);
            Save();

        }

        public Contact GetContact(int id)
        {
            return _context.Contacts.Where(e => e.ContactId == id).FirstOrDefault();
        }

        public ICollection<Contact> GetContacts()
        {
            return _context.Contacts.OrderBy(p => p.ContactId).ToList();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Contact UpdateContact(Contact contact)
        {
            var existingContact = _context.Contacts.FirstOrDefault(c => c.ContactId == contact.ContactId);
            if (existingContact != null)
            {
                _context.Entry(existingContact).CurrentValues.SetValues(contact);
                Save();
                return existingContact;
            }
            else
            {
                throw new InvalidOperationException("Contact not found");
            }
        }

        public ICollection<Contact> GetContactsWithCompanyAndCountry()
        {
            return _context.Contacts
                   .Include(c => c.Company)
                   .Include(c => c.Country)
                   .ToList();
        }

        public ICollection<Contact> FilterContacts(int? countryId, int? companyId)
        {
            var query = _context.Contacts.AsQueryable();

            if (countryId.HasValue)
            {
                query = query.Where(c => c.CountryId == countryId.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(c => c.CompanyId == companyId.Value);
            }

            return query.ToList();
        }
    }
}
