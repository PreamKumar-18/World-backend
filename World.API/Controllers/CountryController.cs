using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using World.API.Data;
using World.API.Models;

namespace World.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public CountryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult<IEnumerable<Country>> GetAll()
        {
            var countries = _dbContext.Countries.ToList();

            if(countries == null)
            {
                return NoContent();
            }
            return Ok(countries);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult<Country> GetById(int id)
        {
            var country = _dbContext.Countries.Find(id);

            if(country == null)
            {
                return NoContent();
            }

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
      
        public ActionResult<Country> Create([FromBody] Country country)
        {
            var result = _dbContext.Countries.AsQueryable().Where(x => x.Name.ToLower().Trim() == country.Name.ToLower().Trim()).Any();

            if(result)
            {
                return Conflict("Country Already Exist in Database");
            }
            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();

            return CreatedAtAction("GetById", new {id = country.Id}, country);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Country> Update(int id,[FromBody] Country country)
        {
            // id valid check

            if(country ==null || id != country.Id)
            {
                return BadRequest();
            }

            // finding record using given id

            var countryFromDb = _dbContext.Countries.Find(id);

            //if in case record not found

            if(countryFromDb == null)
            {
                return NotFound();
            }

            countryFromDb.Name = country.Name;
            countryFromDb.ShortName = country.ShortName;
            countryFromDb.CountryCode = country.CountryCode;

            _dbContext.Countries.Update(countryFromDb);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult DeleteById(int id)
        {
            if(id == 0)
            {
                return BadRequest(); 
            }
           var country = _dbContext.Countries.Find(id);
            if(country == null)
            {
                return NotFound();
            }

            _dbContext.Countries.Remove(country);
            _dbContext.SaveChanges();

            return NoContent();
        }

    }
}
