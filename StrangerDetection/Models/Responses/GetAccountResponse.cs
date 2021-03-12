using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Responses
{
    public class GetAccountResponse
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Image { get; set; }

        public int RoleID { get; set; }

        public string Fullname { get; set; }

        public string Address { get; set; }

        public string FrontIdentityImage { get; set; }

        public string BackIdentityImage { get; set; }

        public bool IsLogin { get; set; }

        public bool IsRemember { get; set; }

        public GetAccountResponse(string username, string image, int roleId, string password, string fullname,
            string address, string frontIdentity, string backIdentity)
        {
            this.Username = username;
            this.Image = image;
            this.RoleID = roleId;
            this.Password = password;
            this.Fullname = fullname;
            this.Address = address;
            this.FrontIdentityImage = frontIdentity;
            this.BackIdentityImage = backIdentity;
        }


    }
}
