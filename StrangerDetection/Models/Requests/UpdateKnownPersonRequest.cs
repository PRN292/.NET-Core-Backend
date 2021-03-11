using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Requests
{
    public class UpdateKnownPersonRequest
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string email { get; set; }
    }
}
