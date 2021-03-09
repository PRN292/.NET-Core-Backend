using System;
using System.Collections.Generic;

#nullable disable

namespace StrangerDetection.Models
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblAccounts = new HashSet<TblAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TblAccount> TblAccounts { get; set; }
    }
}
