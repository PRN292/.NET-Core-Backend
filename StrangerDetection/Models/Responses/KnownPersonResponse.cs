using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Responses
{
    public class KnownPersonResponse
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }

        public List<EncodingResponse> encodingResponseList { get; set; }
    }
}
