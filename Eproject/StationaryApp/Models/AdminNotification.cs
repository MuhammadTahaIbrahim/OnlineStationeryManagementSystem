using System;
using System.Collections.Generic;

namespace StationaryApp.Models
{
    public partial class AdminNotification
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string WholesalerEmail { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
