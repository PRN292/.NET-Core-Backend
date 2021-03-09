using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Responses
{
    public class AuthenticationResponse
    {
        public string username { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        
        public string token { get; set; }

        public AuthenticationResponse(TblAccount account, string token)
        {
            username = account.Username;
            address = account.Address;
            name = account.Name;
            this.token = token;
        }

    }
}
