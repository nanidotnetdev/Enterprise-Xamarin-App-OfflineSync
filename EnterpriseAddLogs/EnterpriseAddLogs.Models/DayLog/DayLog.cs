using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;

namespace EnterpriseAddLogs.Models
{
    public class DayLog:  BaseModel
    {
        public Guid DayLogId { get; set; }

        public string Comment { get; set; }

        public DateTime DateLogged { get; set; }

        public Guid? DayTimeId { get; set; }
    }
}
