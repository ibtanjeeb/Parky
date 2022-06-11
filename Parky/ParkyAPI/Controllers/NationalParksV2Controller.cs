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
    [Route("api/[controller]")]
  
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksV2Controller(INationalParkRepository npRepo,IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get The National Park list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(List<NationalPark>))]
        
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
  

    }
}
