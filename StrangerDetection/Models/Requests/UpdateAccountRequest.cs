using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Requests
{
    public class UpdateAccountRequest
    {
        public string Username { get; set; }

        public string Image { get; set; }

        public int RoleID { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }

        public string Address { get; set; }

        public string FrontIdentity { get; set; }

        public string BackIdentity { get; set; }
    }
}
