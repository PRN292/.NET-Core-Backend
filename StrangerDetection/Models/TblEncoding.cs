using System;
using System.Collections.Generic;

#nullable disable

namespace StrangerDetection.Models
{
    public partial class TblEncoding
    {
        public string Id { get; set; }
        public string ImageName { get; set; }
        public string Encodings { get; set; }
        public string KnownPersonId { get; set; }

        public virtual TblKnownPerson KnownPerson { get; set; }
    }
}
