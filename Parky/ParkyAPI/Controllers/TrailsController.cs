using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/[Trails]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;
        public TrailsController(ITrailRepository trailRepo,IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get The Trail list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(List<Trail>))]
        
        public IActionResult GetTrails()
        {
            var objlist = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();

            foreach(var obj in objlist)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
                
            }
            return Ok(objDto);
        }
        /// <summary>
        /// Get Individual Trail
        /// </summary>
        /// <param name="trailid">The Id of Trail</param>
        /// <returns></returns>
        [HttpGet("{trailid:int}", Name= "GetTrail")]
        [ProducesResponseType(200, Type = typeof(Trail))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailid)
        {
            var obj = _trailRepo.GetTrail(trailid);
            if(obj==null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);

        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody]TrailDto trailDto)
        {

            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            if(_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists");
                return StatusCode(404, ModelState);

            }
            
            var trailobj = _mapper.Map<Trail>(trailDto);

            if(!_trailRepo.CreatedTrail(trailobj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{trailobj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { trailid = trailobj.Id }, trailobj);

        }
        [HttpPatch("{trailid:int}", Name = "UpdateTrail")]

        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailid, [FromBody]TrailDto trailDto)
        {
            if (trailDto == null|| trailid != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            var trailobj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.UpdateTrail(trailobj))
            {
                ModelState.AddModelError("", $"Something went wrong when Updating the record{trailobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
        [HttpDelete("{trailid:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailid)
        {
            if(!_trailRepo.TrailExists(trailid))
            {
                return NotFound();
            }
            var trailobj = _trailRepo.GetTrail(trailid);

            if (!_trailRepo.DeleteTrail(trailobj))
            {
                ModelState.AddModelError("", $"Something went wrong when Deleting the record{trailobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }

    }
}
