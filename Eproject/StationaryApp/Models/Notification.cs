using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string WholesalerEmail { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}
