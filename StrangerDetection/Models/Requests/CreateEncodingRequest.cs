using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Requests
{
    public class CreateEncodingRequest
    {
        public string knownPersonID { get; set; }
        public string image { get; set; }
    }
}
