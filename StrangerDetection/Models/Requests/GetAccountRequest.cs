using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Requests
{
    public class GetAccountRequest
    {
        [Required]
        public string Username { get; set; }
    }
}
