using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class Product
    {
        public Product()
        {
            Retailerorders = new HashSet<Retailerorder>();
            Whorders = new HashSet<Whorder>();
        }

        public int ProId { get; set; }
        public string? ProName { get; set; }
        public string? ProDesc { get; set; }
        public string? ProPrice { get; set; }
        public string? ProImg { get; set; }
        public int? ProcatidFk { get; set; }
        public string? Availability { get; set; }

        public virtual Addcategory? ProcatidFkNavigation { get; set; }
        public virtual ICollection<Retailerorder> Retailerorders { get; set; }
        public virtual ICollection<Whorder> Whorders { get; set; }
    }
}
