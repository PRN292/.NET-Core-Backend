using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Responses
{
    public class AccountResponse
    {
        public string Fullname { get; set; }

        public string Image { get; set; }


        public AccountResponse(string fullname, string image)
        {
            this.Fullname = fullname;
            this.Image = image;
        }

    }
}
