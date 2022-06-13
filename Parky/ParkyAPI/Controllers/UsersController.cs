using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/Users")]

    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public UsersController(IUserRepository userRepository)
        {
           _userRepo= userRepository;

        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User Model)
        {
            var user = _userRepo.Authenticate(Model.Username, Model.Password);
            if(user==null)
            {
                return BadRequest(new { mesaage = "User or Password is Incorrect" });

            }
            return Ok(user);

        }


    }
}
