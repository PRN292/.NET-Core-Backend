using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrangerDetection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrangerDetection.Models.Requests;
using StrangerDetection.Helpers;

namespace StrangerDetection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase

    {
        private readonly IUserService userService;
        public UsersController(IUserService service)
        {
            this.userService = service;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticattionRequest model)
        {
            var response = userService.Authenticate(model);
            if (response == null)
            {
                return BadRequest(new { message = "Username or Password incorrect"});
            }
            return Ok(response);
        }

        [Authorize]
        //This the our own Authorize Attibute(Annotation) that defined in Helpers.
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout(String username)
        {
            var result = userService.LogoutUser(username);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        public IActionResult UpdateAccount(AccountRequest model)
        {
            
        }

    }

}
