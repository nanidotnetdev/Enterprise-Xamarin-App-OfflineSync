﻿namespace EnterpriseAddLogs.Models
{
    using System;

    public class LogTypeEntity
    {
        public Guid LogTypeID { get; set; }

        public string LogType { get; set; }

        public Guid? ProductgroupID { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}