using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Models.Requests
{
    public class CreateAccountRequest
    {
        // required fields (7 items)
        [Required]
        public string Username { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string FrontIdentityImage { get; set; }

        [Required]
        public string BackIdentityImage { get; set; }

        [Required]
        public string KnowPersonId { get; set; }

        //non - required fields (2 items)
        public string Address { get; set; }

        public string FullName { get; set; }


    }
}
