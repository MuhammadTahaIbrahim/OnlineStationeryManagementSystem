using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class ProDetail
    {
        public int ProId { get; set; }
        public string? ProName { get; set; }
        public string? ProDesc { get; set; }
        public string? ProPrice { get; set; }
        public string? ProImg { get; set; }
        public string? Availability { get; set; }
        public string? CatName { get; set; }
        public string? CatDesc { get; set; }
    }
}
