using Microsoft.Azure.Mobile.Server;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseAddLogs.Backend.DataObjects
{
    [Table("DayLog")]
    public class DayLog: EntityData
    {
        public string Comment { get; set; }

        public DateTime DateLogged { get; set; }

        public Guid? DayTimeId { get; set; }

        public string CreatedBy { get; set; }

    }
}