using Microsoft.AspNetCore.Mvc;
using StrangerDetection.Helpers;
using StrangerDetection.Models;
using StrangerDetection.Models.Requests;
using StrangerDetection.Models.Responses;
using StrangerDetection.Services;
using StrangerDetection.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;

namespace StrangerDetection.Controllers
{

    [Route("/api/v1/[controller]")]
    [ApiController]
    public class KnownPersonsController : Controller
    {
        private static string ApiKey = "AIzaSyASmQKmEj0zSq9sZbzodY-NS_SzUbBQnss";
        private static string bucket = "gs://strangerdetection.appspot.com";
        private static string AuthEmail = "bangmapleproject@gmail.com";
        private static string AuthPassword = "13062000";
        private readonly IWebHostEnvironment _env;
        private IKnownPersonService personService;
        private IEncodingService encodingService;

        public KnownPersonsController(IWebHostEnvironment env, IKnownPersonService personService, IEncodingService encodingService)
        {
            this.encodingService = encodingService;
            this.personService = personService;
            _env = env;
        }


        [Authorize(Constant.Role.ADMIN)]
        [HttpGet]
        public IActionResult GetAllKnownPersons()
        {
            List<TblKnownPerson> knowPersonList = personService.GetAllKnowPerson();
            List<KnownPersonResponse> resultList = new List<KnownPersonResponse>();
            FileStream fs;
            FileStream ms;
            knowPersonList.ForEach(s =>
            {
                List<EncodingResponse> encodingResponses = new List<EncodingResponse>();
                s.TblEncodings.ToList().ForEach(e =>
                {
                    //TODO: get image base64 from firebase
                    string folderName = "Favicon";
                    string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var a =  auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword).Result;
                    
                    var cancellation = new CancellationTokenSource();
                    
                    var task = new FirebaseStorage(
                            bucket,
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                            })
                        .Child($"aspcore.png")
                        .GetDownloadUrlAsync().Result;
                    Console.WriteLine(task);
                    //task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");



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
            if (KnowPersonValidator.ValidateUpdateKnowPersonRequestObj(request))
            {
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
                    return Ok(new { message = "Update knownperson successfully"});
                }
            }

            return BadRequest(new { message = "Update knownperson failed"});
        }

        [Authorize(Constant.Role.ADMIN)]
        [HttpPost]
        public IActionResult CreateKnownPerson(CreateKnownPersonRequest request)
        {
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
                return Ok(new { message = "Create knownperson successfully"});
            }
            return BadRequest(new { message = "Create knownperson failed" });

        }

        [Authorize(Constant.Role.ADMIN)]
        [HttpDelete("{email}")]
        public IActionResult DeleteKnownPerson(string email)
        {
            bool result = personService.DeleteKnownPerson(email);
            if (result)
            {
                return Ok(new { message = "Delete knowperson successfully"});
            }
            return BadRequest(new { message = "Delete knownperson failed"});
        }



        [Authorize(Constant.Role.ADMIN)]
        [HttpDelete]
        [Route("Encodings")]
        public IActionResult DeleteEncodingByID(string ID)
        {
            return Ok(new { message = "Delete encoding successfully"});
        }

        [Authorize(Constant.Role.ADMIN)]
        [HttpPost("encodings")]
        public IActionResult CreateNewEncoding(CreateEncodingRequest request)
        {
            if (EncodingValidator.ValidateCreateEncodingRequest(request.knownPersonEmail, request.image))
            {
                bool result = encodingService.CreateEncoding(request.knownPersonEmail, request.image);
                if (result)
                {
                    return StatusCode(201);
                }
            }

            return BadRequest(new { message = "Create encoding failed"});

        }




    }
}
