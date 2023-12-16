using AutoMapper;
using BasicWebAPI.Dto;
using BasicWebAPI.Interfaces;
using BasicWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            this._companyService = companyService;   
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]

        public IActionResult GetCompanies() {
            var companies = _mapper.Map<List<CompanyDto>>(_companyService.GetCompanies());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public IActionResult GetCompany(int id)
        {
            var company = _companyService.GetCompany(id);
            if (company == null)
            {
                return NotFound();
            }
            var companyDto = _mapper.Map<CompanyDto>(company);
            return Ok(companyDto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateCompany([FromBody] CompanyDto companyCreate)
        {
            if (companyCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var companyMap = _mapper.Map<Company>(companyCreate);

            int newCompanyId = _companyService.CreateCompany(companyMap);
            if (newCompanyId == 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving the new company");
                return StatusCode(500, ModelState); 
            }

            var newCompany = _companyService.GetCompany(newCompanyId);
            var newCompanyDto = _mapper.Map<CompanyDto>(newCompany);

            return CreatedAtAction(nameof(GetCompany), new { id = newCompanyId }, newCompanyDto);
        }

        [HttpPut("{companyId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCompany(int companyId, [FromBody] CompanyDto updatedCompany)
        {
            if (updatedCompany == null)
                return BadRequest(ModelState);

            if (companyId != updatedCompany.CompanyId)
                return BadRequest(ModelState);

            if (!_companyService.CompanyExists(companyId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var companyToUpdate = _mapper.Map<Company>(updatedCompany);

            var updateCompany = _companyService.UpdateCompany(companyToUpdate);

            if (updateCompany == null)
                return StatusCode(500, "A problem happened while handling your request.");

            var updatedCompanyResult = _mapper.Map<CompanyDto>(updateCompany);
            return Ok(updatedCompanyResult);
        }

        [HttpDelete("{companyId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCompany(int companyId)
        {
            if (!_companyService.CompanyExists(companyId))
            {
                return NotFound();
            }

            var companyToDelete = _companyService.GetCompany(companyId);

            _companyService.DeleteCompany(companyToDelete);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            

            return NoContent();
        }
    }
}
