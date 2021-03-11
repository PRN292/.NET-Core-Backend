using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrangerDetection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrangerDetection.Models.Requests;
using StrangerDetection.Helpers;
using System.Net;

namespace StrangerDetection.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase

    {
        private readonly IUserService userService;
        public AccountsController(IUserService service)
        {
            this.userService = service;
        }

        [HttpPost("login")]
        public IActionResult Authenticate(AuthenticationRequest model)
        {
            var response = userService.Authenticate(model);
            if (response == null)
            {
                return BadRequest(new { message = "Username or Password incorrect" });
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = userService.GetAllFullnameAndImage();
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest();
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

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateAccountRequest obj)
        {
            var result = userService.CreateAccount(obj);
            if (result)
            {
                return StatusCode(201);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("getaccount")]
        public IActionResult GetAccount(string username)
        {
            var result = userService.GetAnAccount(username);
            if (result != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        //[Authorize]
        //[HttpPatch("update")]
        //public IActionResult UpdateAccount(UpdateAccountRequest obj)
        //{
        //    var result = UserService.SelfUpdateAccount(obj);
        //    if (result)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

        //[Authorize]
        //[HttpDelete("delete")]
        //public 

    }

}
