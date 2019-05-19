﻿using System;
using Newtonsoft.Json;

namespace EnterpriseAddLogs.Models
{
    public class DayLog:  BaseModel
    {
        public Guid DayLogId { get; set; }

        public string Comment { get; set; }

        public DateTime DateLogged { get; set; }

        public Guid? DayTimeId { get; set; }

        //don't persist to remote.
        [JsonIgnore]
        public bool IsNew { get; set; }
    }
}
