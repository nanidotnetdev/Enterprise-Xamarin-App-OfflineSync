using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;

namespace EnterpriseAddLogs.Models
{
    public class DayLog
    {
        public Guid DayLogId { get; set; }

        public string Comment { get; set; }

        public DateTime DateLogged { get; set; }

        public Guid? DayTime { get; set; }

        public string id { get; set; }

        [Version]
        public byte[] Version { get; set; }

        [CreatedAt]
        public DateTimeOffset CreatedAt { get; set; }

        [UpdatedAt]
        public DateTimeOffset? UpdatedAt { get; set; }

        [Deleted]
        public bool Deleted { get; set; }
    }
}
