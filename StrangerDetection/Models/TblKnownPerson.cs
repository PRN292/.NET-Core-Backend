using System;
using System.Collections.Generic;

#nullable disable

namespace StrangerDetection.Models
{
    public partial class TblKnownPerson
    {
        public TblKnownPerson()
        {
            TblAccounts = new HashSet<TblAccount>();
            TblEncodings = new HashSet<TblEncoding>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public virtual ICollection<TblAccount> TblAccounts { get; set; }
        public virtual ICollection<TblEncoding> TblEncodings { get; set; }
    }
}
