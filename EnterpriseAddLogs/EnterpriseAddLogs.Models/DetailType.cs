using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseAddLogs.Models
{
    public class DetailTypeEntity
    {
        public Guid DetailTypeId { get; set; }

        public string DetailType { get; set; }

        public bool IsActive { get; set; }
    }
}
