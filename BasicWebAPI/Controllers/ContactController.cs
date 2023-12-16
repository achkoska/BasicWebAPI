using AutoMapper;
using BasicWebAPI.Dto;
using BasicWebAPI.Interfaces;
using BasicWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        public ContactController(IContactService contactService, IMapper mapper)
        {
            this._contactService = contactService;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Contact>))]

        public IActionResult GetContacts()
        {
            var contacts = _mapper.Map<List<ContactDto>>(_contactService.GetContacts());
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(contacts);

        }

        [HttpGet("{id}")]
        public IActionResult GetContact(int id)
        {
            var contact = _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }
            var contactDto = _mapper.Map<ContactDto>(contact);
            return Ok(contactDto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateContact([FromBody] ContactDto contactCreate)
        {
            if (contactCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactMap = _mapper.Map<Contact>(contactCreate);

            int newContactId = _contactService.CreateContact(contactMap);
            if (newContactId == 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving the new contact");
                return StatusCode(500, ModelState); 
            }

            var newContact = _contactService.GetContact(newContactId);
            var newContactDto = _mapper.Map<ContactDto>(newContact);

            return CreatedAtAction(nameof(GetContact), new { id = newContactId }, newContactDto);
        }


        [HttpPut("{contactId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateContact(int contactId, [FromBody] ContactDto updatedContact)
        {
            if (updatedContact == null)
                return BadRequest(ModelState);

            if (contactId != updatedContact.ContactId)
                return BadRequest(ModelState);

            if (!_contactService.ContactExists(contactId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var contactToUpdate = _mapper.Map<Contact>(updatedContact);

            var updateContact = _contactService.UpdateContact(contactToUpdate);

            if (updateContact == null)
                return StatusCode(500, "A problem happened while handling your request.");

            var updatedContactResult = _mapper.Map<ContactDto>(updateContact);
            return Ok(updatedContactResult);
        }

        [HttpDelete("{contactId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteContact(int contactId)
        {
            if (!_contactService.ContactExists(contactId))
            {
                return NotFound();
            }

            var contactToDelete = _contactService.GetContact(contactId);

            _contactService.DeleteContact(contactToDelete);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            

            return NoContent();
        }

        [HttpGet("WithDetails")]
        public IActionResult GetContactsWithCompanyAndCountry()
        {
            var contacts = _contactService.GetContactsWithCompanyAndCountry();

            var contactsWithDetailsDtos = _mapper.Map<IEnumerable<ContactDetailsDto>>(contacts);

            return Ok(contactsWithDetailsDtos);
        }

        [HttpGet("FilterContacts")]
        public IActionResult FilterContacts([FromQuery] int? countryId, [FromQuery] int? companyId)
        {
            var filteredContacts = _contactService.FilterContacts(countryId, companyId);
            var contactDtos = _mapper.Map<IEnumerable<ContactDto>>(filteredContacts);
            return Ok(contactDtos);
        }

    }
}
