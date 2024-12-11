using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class Whorder
    {
        public int WorderId { get; set; }
        public int? WproIdfk { get; set; }
        public string? Catename { get; set; }
        public int? Wproqty { get; set; }
        public string? Wtotalprice { get; set; }
        public string? Wstatus { get; set; }
        public string? Whsaleremail { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual Product? WproIdfkNavigation { get; set; }
    }
}
