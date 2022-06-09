﻿using AutoMapper;
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
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository npRepo,IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get The National Park list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetNationalPark()
        {
            var objlist = _npRepo.GetNationalParks();
            var objDto = new List<NationalParkDto>();

            foreach(var obj in objlist)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
                
            }
            return Ok(objDto);
        }
        /// <summary>
        /// Get Individual National Park
        /// </summary>
        /// <param name="nationalParkid">The Id of National Park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkid:int}", Name= "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkid)
        {
            var obj = _npRepo.GetNationalPark(nationalParkid);
            if(obj==null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);

        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {

            if (nationalParkDto==null)
            {
                return BadRequest(ModelState);
            }

            if(_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);

            }
            
            var nationalparkobj = _mapper.Map<NationalPark>(nationalParkDto);

            if(!_npRepo.CreatedNatonalPark(nationalparkobj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record{nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkid = nationalparkobj.Id }, nationalparkobj);

        }
        [HttpPatch("{nationalParkid:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkid,[FromBody]NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null|| nationalParkid !=nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            var nationalparkobj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.UpdateNationalPark(nationalparkobj))
            {
                ModelState.AddModelError("", $"Something went wrong when Updating the record{nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
        [HttpDelete("{nationalParkid:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkid)
        {
            if(!_npRepo.NationalParkExists(nationalParkid))
            {
                return NotFound();
            }
            var nationalparkobj = _npRepo.GetNationalPark(nationalParkid);

            if (!_npRepo.DeleteNatonalPark(nationalparkobj))
            {
                ModelState.AddModelError("", $"Something went wrong when Deleting the record{nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }

    }
}
