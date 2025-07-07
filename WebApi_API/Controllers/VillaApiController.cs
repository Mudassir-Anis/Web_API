using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_API.Data;
using WebApi_API.Models;
using WebApi_API.Models.DTO;

namespace WebApi_API.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaApiController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<VilaDto>> GetVillas()
        {
            
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VilaDto> GetVilla(int id)
        {
            if (id == 0)
            {

                return BadRequest();
            }
            var vila = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (vila == null)
            {
                return NotFound();
            }
            return Ok(vila);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<VilaDto> CreateVilla(VilaDto vilaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vilaDto == null)
            {
                return BadRequest();
            }
            if (vilaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            int Id = _db.Villas.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            
            var vila = _mapper.Map<Vila>(vilaDto);
            _db.Villas.Add(vila);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new { id = Id }, vila);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var vila = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (vila == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(vila);
            _db.SaveChanges();
            return NoContent();
        }
        [HttpPut("{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VilaDto vilaDto)
        {
            if (vilaDto == null || id != vilaDto.Id)
            {
                return BadRequest();
            }

            var vila = _db.Villas.FirstOrDefault(u => u.Id == id);
            if(vila == null)
            {
                return BadRequest();
            }
            //vila.Name = vilaDto.Name;
            //vila.Occupancy = vilaDto.Occupancy;
            //vila.Sqft = vilaDto.Sqft;
            _mapper.Map(vilaDto,vila);

            _db.Villas.Update(vila);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VilaDto> vilaPatch)
        {
            if (vilaPatch == null || id == 0)
            {
                return BadRequest();
            }

            var vila = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if(vila == null)
            {
                return NotFound();
            }
            var vilaDto = _mapper.Map<VilaDto>(vila);
            vilaPatch.ApplyTo(vilaDto, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //_mapper.Map(vilaDto,vila);
            vila = _mapper.Map<Vila>(vilaDto);
            _db.Villas.Update(vila);
            _db.SaveChanges();
            return NoContent();
        }


    }
}
