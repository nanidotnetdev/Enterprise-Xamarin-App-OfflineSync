using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseAddLogs.Models
{
    public class ComponentEntity
    {
        public Guid ComponentId { get; set; }

        public string ComponentName { get; set; }

        public bool IsActive { get; set; }
    }
}
