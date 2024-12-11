using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class Retailerorder
    {
        public int RorderId { get; set; }
        public int? RproIdfk { get; set; }
        public string? Catename { get; set; }
        public int? Rproqty { get; set; }
        public string? Rtotalprice { get; set; }
        public string? Rstatus { get; set; }
        public string? Retaileremail { get; set; }
        public DateTime? OrderDate { get; set; }

        public virtual Product? RproIdfkNavigation { get; set; }
    }
}
