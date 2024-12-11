using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class Retailerorderlist
    {
        public int RorderId { get; set; }
        public string? Catename { get; set; }
        public int? Rproqty { get; set; }
        public string? Rtotalprice { get; set; }
        public string? Rstatus { get; set; }
        public string? Retaileremail { get; set; }
        public string? ProName { get; set; }
        public string? ProDesc { get; set; }
        public string? ProPrice { get; set; }
        public string? ProImg { get; set; }
    }
}
