using Microsoft.AspNetCore.Mvc;
using StrangerDetection.Helpers;
using StrangerDetection.Models;
using StrangerDetection.Models.Requests;
using StrangerDetection.Models.Responses;
using StrangerDetection.Services;
using StrangerDetection.Validators;
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
        private IEncodingService encodingService;

        public KnownPersonsController(IKnownPersonService personService, IEncodingService encodingService)
        {
            this.encodingService = encodingService;
            this.personService = personService;
        }


        [Authorize(Constant.Role.ADMIN)]
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
                resultList.Add(new KnownPersonResponse
                {
                    address = s.Address,
                    email = s.Email,
                    name = s.Name,
                    phone = s.PhoneNumber,
                    encodingResponseList = encodingResponses
                });
            });

            return Ok(resultList);
        }

        [Authorize(Constant.Role.ADMIN)]
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
            KnownPersonResponse response = new KnownPersonResponse
            {
                email = knownPerson.Email,
                address = knownPerson.Address,
                name = knownPerson.Name,
                phone = knownPerson.PhoneNumber,
                encodingResponseList = encodingResponses
            };
            return Ok(response);
        }

        [Authorize(Constant.Role.ADMIN)]
        [HttpPatch]
        public IActionResult UpdateKnownPerson(UpdateKnownPersonRequest request)
        {
            //TODO: validate request
            if (KnowPersonValidator.ValidateUpdateKnowPersonRequestObj(request))
            {
                //
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
            }

            return BadRequest();
        }

        [Authorize(Constant.Role.ADMIN)]
        [HttpPost]
        public IActionResult CreateKnownPerson(CreateKnownPersonRequest request)
        {
            //TODO: validate request
            if (KnowPersonValidator.ValidateCreateKnowPersonRequestObj(request))
            {
                TblKnownPerson newPerson = new TblKnownPerson
                {
                    Address = request.address,
                    Email = request.email,
                    Name = request.name,
                    PhoneNumber = request.phoneNumber
                };
                bool result = personService.CreateKnownPerson(newPerson);
                return Ok(result);
            }
            return BadRequest();

        }

        [Authorize(Constant.Role.ADMIN)]
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



        [Authorize(Constant.Role.ADMIN)]
        [HttpDelete]
        [Route("Encodings")]
        public IActionResult DeleteEncodingByID(string ID)
        {
            return Ok();
        }

        [Authorize(Constant.Role.ADMIN)]
        [HttpPost("encodings")]
        public IActionResult CreateNewEncoding(CreateEncodingRequest request)
        {
            //TODO: validate request
            if (EncodingValidator.ValidateCreateEncodingRequest(request.knownPersonEmail, request.image))
            {
                bool result = encodingService.CreateEncoding(request.knownPersonEmail, request.image);
                if (result)
                {
                    return StatusCode(201);
                }
            }

            return BadRequest();

        }




    }
}
