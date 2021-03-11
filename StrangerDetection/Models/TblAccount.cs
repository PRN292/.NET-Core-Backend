using System;
using System.Collections.Generic;

#nullable disable

namespace StrangerDetection.Models
{
    public partial class TblAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string IdentificationCardFrontImageName { get; set; }
        public string IdentificationCardBackImageName { get; set; }
        public string ProfileImageName { get; set; }
        public int RoleId { get; set; }
        public string KnownPersonEmail { get; set; }
        public bool? IsLogin { get; set; }
        public bool? IsRemember { get; set; }

        public virtual TblKnownPerson KnownPersonEmailNavigation { get; set; }
        public virtual TblRole Role { get; set; }
    }
}
