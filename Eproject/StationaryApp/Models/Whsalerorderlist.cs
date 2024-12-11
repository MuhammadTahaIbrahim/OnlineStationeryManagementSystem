using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class Whsalerorderlist
    {
        public int WorderId { get; set; }
        public string? Catename { get; set; }
        public int? Wproqty { get; set; }
        public string? Wtotalprice { get; set; }
        public string? Wstatus { get; set; }
        public string? Whsaleremail { get; set; }
        public string? ProName { get; set; }
        public string? ProDesc { get; set; }
        public string? ProPrice { get; set; }
        public string? ProImg { get; set; }
    }
}
