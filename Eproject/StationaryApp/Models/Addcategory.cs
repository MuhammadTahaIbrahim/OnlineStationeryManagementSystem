using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class Addcategory
    {
        public Addcategory()
        {
            Products = new HashSet<Product>();
        }

        public int CatId { get; set; }
        public string? CatName { get; set; }
        public string? CatDesc { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
