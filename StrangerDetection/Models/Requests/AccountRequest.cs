using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Requests
{
    public class AccountRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string IdentificationCardFrontImageName { get; set; }
        public string IdentificationCardBackImageName { get; set; }
        public string ProfileImageName { get; set; }
        
        
    }
}
