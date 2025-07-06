using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi_API.Data;
using WebApi_API.Logging;
using WebApi_API.Models;
using WebApi_API.Models.DTO;

namespace WebApi_API.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        public readonly ILogging _logger;
        public VillaApiController(ILogging logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public ActionResult<VilaDto> GetVillas()
        {
            _logger.Log("Getting all villas","success");
            return Ok(VilaStore.vilaList);
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VilaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log($"Invalid villa ID provided: {id}","error");
                return BadRequest();
            }
            var vila = VilaStore.vilaList.FirstOrDefault(u => u.Id == id);
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
            vilaDto.Id = VilaStore.vilaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VilaStore.vilaList.Add(vilaDto);
            return CreatedAtRoute("GetVilla", new { id = vilaDto.Id }, vilaDto);
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
            var vila = VilaStore.vilaList.FirstOrDefault(u => u.Id == id);
            if (vila == null)
            {
                return NotFound();
            }
            VilaStore.vilaList.Remove(vila);
            return NoContent();
        }
        [HttpPut("{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VilaDto vilaDto)
        {
            if (vilaDto == null || id != vilaDto.Id)
            {
                return BadRequest();
            }
            var vila = VilaStore.vilaList.FirstOrDefault(u => u.Id == id);
            vila.Name = vilaDto.Name;
            vila.Occupancy = vilaDto.Occupancy;
            vila.SqFt = vilaDto.SqFt;
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

            var vila = VilaStore.vilaList.FirstOrDefault(u => u.Id == id);
            if(vila == null)
            {
                return NotFound();
            }
            vilaPatch.ApplyTo(vila, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
