using AutoMapper;
using BasicWebAPI.Dto;
using BasicWebAPI.Interfaces;
using BasicWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountryController(ICountryService countryService, IMapper mapper)
        {
            this._countryService = countryService;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
    
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryService.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public IActionResult GetCountry(int id)
        {
            var country = _countryService.GetCountry(id);
            if (country == null)
            {
                return NotFound();
            }
            var countryDto = _mapper.Map<CountryDto>(country);
            return Ok(countryDto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countrytMap = _mapper.Map<Country>(countryCreate);

            int newCountryId = _countryService.CreateCountry(countrytMap);
            if (newCountryId == 0)
            {
                ModelState.AddModelError("", "Something went wrong while saving the new country");
                return StatusCode(500, ModelState); 
            }

            var newCountry = _countryService.GetCountry(newCountryId);
            var newCountrytDto = _mapper.Map<CountryDto>(newCountry);

            return CreatedAtAction(nameof(GetCountry), new { id = newCountryId }, newCountrytDto);
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.CountryId)
                return BadRequest(ModelState);

            if (!_countryService.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryToUpdate = _mapper.Map<Country>(updatedCountry);

            var updateCountry = _countryService.UpdateCountry(countryToUpdate);

            if (updateCountry == null)
                return StatusCode(500, "A problem happened while handling your request.");

            var updatedCountryResult = _mapper.Map<CountryDto>(updateCountry);
            return Ok(updatedCountryResult);
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryService.CountryExists(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _countryService.GetCountry(countryId);
            _countryService.DeleteCountry(countryToDelete);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();
        }
    }
}
