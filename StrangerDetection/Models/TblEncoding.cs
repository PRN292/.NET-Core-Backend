using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace StrangerDetection.Models
{
    public partial class TblEncoding
    {
        public string Id { get; set; }
        public string ImageName { get; set; }
        public string KnownPersonEmail { get; set; }
        [JsonIgnore]
        public virtual TblKnownPerson KnownPersonEmailNavigation { get; set; }
    }
}
