using Microsoft.AspNetCore.Mvc;
using StrangerDetection.Helpers;
using StrangerDetection.Models;
using StrangerDetection.Models.Requests;
using StrangerDetection.Models.Responses;
using StrangerDetection.Services;
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
        private IKnownPersonService personService;

        public KnownPersonsController(IKnownPersonService personService)
        {
            this.personService = personService;
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetAllKnownPersons()
        {
            List<TblKnownPerson> knowPersonList = personService.GetAllKnowPerson();
            List<KnownPersonResponse> resultList = new List<KnownPersonResponse>();
            knowPersonList.ForEach(s =>
            {
                List<EncodingResponse> encodingResponses = new List<EncodingResponse>();
                s.TblEncodings.ToList().ForEach(e =>
                {
                    //TODO: get image base64 from firebase
                    encodingResponses.Add(new EncodingResponse { ID = e.Id, image = e.ImageName });
                });
                resultList.Add(new KnownPersonResponse {
                    address = s.Address,
                    email = s.Email,
                    name = s.Name,
                    phone = s.PhoneNumber,
                    encodingResponseList = encodingResponses
                });
            });

            return Ok(resultList);
        }

        [Authorize]
        [HttpGet("{email}")]
        public IActionResult GetKnownPersonByEmail(string email)
        {
            TblKnownPerson knownPerson = personService.GetKnownPersonByEmail(email);
            List<EncodingResponse> encodingResponses = new List<EncodingResponse>();
            knownPerson.TblEncodings.ToList().ForEach(e =>
            {
                //TODO: get image base64 from firebase
                encodingResponses.Add(new EncodingResponse { ID = e.Id, image = e.ImageName });
            });
            KnownPersonResponse response = new KnownPersonResponse {
                email = knownPerson.Email,
                address = knownPerson.Address,
                name = knownPerson.Name,
                phone = knownPerson.PhoneNumber,
                encodingResponseList = encodingResponses
            };
            return Ok(response);
        }

        [Authorize]
        [HttpPatch]
        public IActionResult UpdateKnownPerson(UpdateKnownPersonRequest request)
        {
            //TODO: validate request
            TblKnownPerson newPerson = new TblKnownPerson
            {
                Address = request.address,
                Email = request.email,
                Name = request.name,
                PhoneNumber = request.phoneNumber
            };
            bool result = personService.UpdateKnownPerson(newPerson);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateKnownPerson(CreateKnownPersonRequest request)
        {
            TblKnownPerson newPerson = new TblKnownPerson
            {
                Address = request.address,
                Email = request.email,
                Name = request.name,
                PhoneNumber = request.phoneNumber
            };
            bool result = personService.CreateKnownPerson(newPerson);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{email}")]
        public IActionResult DeleteKnownPerson(string email)
        {
            bool result = personService.DeleteKnownPerson(email);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }



        [Authorize]
        [HttpDelete]
        [Route("Encodings")]
        public IActionResult DeleteEncodingByID(string ID)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("Encodings")]
        public IActionResult CreateNewEncoding(CreateEncodingRequest request)
        {
            return StatusCode(201);
        }

        [Authorize]
        [HttpDelete]
        [Route("Encodings")]
        public IActionResult DeleteEncoding(string ID)
        {
            return Ok();
        }


    }
}
