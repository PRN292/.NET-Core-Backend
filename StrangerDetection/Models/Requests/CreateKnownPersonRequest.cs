using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Requests
{
    public class CreateKnownPersonRequest
    {
        [Required]
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        [Required]
        public string email { get; set; }
    }
}
