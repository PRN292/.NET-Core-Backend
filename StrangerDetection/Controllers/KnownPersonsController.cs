using Microsoft.AspNetCore.Mvc;
using StrangerDetection.Helpers;
using StrangerDetection.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Controllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class KnownPersonsController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetAllKnownPersons()
        {
            return Ok();            
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetKnownPersonById(string id)
        {
            return Ok();
        }

        [Authorize]
        [HttpPatch]
        public IActionResult UpdateKnownPerson(UpdateKnownPersonRequest request)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateKnownPerson(CreateKnownPersonRequest request)
        {
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteKnownPerson(string knownPersonID)
        {
            return Ok();
        }




        [Authorize]
        [HttpDelete]
        [Route("Encoding")]
        public IActionResult DeleteEncodingByID(string id)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("Encoding")]
        public IActionResult CreateNewEncoding(CreateEncodingRequest request)
        {
            return StatusCode(201);
        }

        [Authorize]
        [HttpDelete]
        [Route("Encoding")]
        public IActionResult DeleteEncoding(string KnownPersonID)
        {
            return Ok();
        }


    }
}
