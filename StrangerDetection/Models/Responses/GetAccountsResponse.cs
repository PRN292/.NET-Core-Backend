using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Responses
{
<<<<<<< HEAD:StrangerDetection/Models/Responses/GetAccountsResponse.cs
    public class GetAccountsResponse
=======
    public class AccountResponse
>>>>>>> 366b5cd93e32837ced9e9868d443f5e12674442a:StrangerDetection/Models/Responses/AccountResponse.cs
    {
        public string Fullname { get; set; }

        public string Image { get; set; }


<<<<<<< HEAD:StrangerDetection/Models/Responses/GetAccountsResponse.cs
        public GetAccountsResponse(string fullname, string image)
=======
        public AccountResponse(string fullname, string image)
>>>>>>> 366b5cd93e32837ced9e9868d443f5e12674442a:StrangerDetection/Models/Responses/AccountResponse.cs
        {
            this.Fullname = fullname;
            this.Image = image;
        }

    }
}
