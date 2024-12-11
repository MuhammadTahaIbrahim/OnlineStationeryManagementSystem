using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class RetailerNotification
    {
        public int Id { get; set; }
        public string RetailerEmail { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}
