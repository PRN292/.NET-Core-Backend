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
using StrangerDetection.Models.Responses;

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
            AuthenticationResponse response = userService.Authenticate(model);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest(new { message = "Username or Password incorrect" });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<GetAccountsResponse> response = userService.GetAllFullnameAndImage();
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout(string username)
        {
            bool result = userService.LogoutUser(username);
            if (result)
            {
                return Ok();
            }
            return BadRequest(new { message = "Logout failed"});
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
            return BadRequest(new  { message = "Create account failed" });
        }

        [Authorize]
        [HttpGet("{username}")]
        public IActionResult GetAccount(string username)
        {
            GetAccountResponse result = userService.SearchAccountByUsername(username);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(new { message = "Account doesn't exist"});
        }

        [Authorize]
        [HttpPatch]
        public IActionResult UpdateAccount(UpdateAccountRequest obj)
        {
            bool result = userService.UpdateAccount(obj);
            if (result)
            {
                return Ok();
            }
            return BadRequest(new {message ="Account doesn't exist. Update failed" });
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteAccount(string username)
        {
            bool result = userService.DeleteAccount(username);
            if (result)
            {
                return Ok();
            }
            return BadRequest(new { message = "Account doesn't exist. Delete Failed" });
        }

    }

}
