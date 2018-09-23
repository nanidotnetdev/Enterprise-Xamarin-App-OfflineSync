using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseAddLogs.Models
{
    public class SystemEntity
    {
        public Guid SystemId { get; set; }

        public string SystemName { get; set; }

        public bool IsActive { get; set; }
    }
}
