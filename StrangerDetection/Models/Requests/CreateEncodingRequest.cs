using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Requests
{
    public class CreateEncodingRequest
    {
        [Required]
        public string knownPersonEmail { get; set; }
        [Required]
        public string image { get; set; }
    }
}
