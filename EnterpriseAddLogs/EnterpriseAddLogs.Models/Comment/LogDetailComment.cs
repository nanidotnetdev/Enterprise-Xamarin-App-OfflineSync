using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseAddLogs.Models
{
    public class LogDetailComment
    {
        public Guid DetailCommentId { get; set; }

        public Guid SystemId { get; set; }

        public string SystemName { get; set; }

        public Guid? ComponentId { get; set; }

        public Guid? LogDetailTypeId { get; set; }

        public string LogDetailType { get; set; }

        public string ComponentName { get; set; }

        public double? Miles { get; set; }

        public double? Hours { get; set; }

        public double? Cycles { get; set; }

        public string Comment { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public string CommentHeader {
            get
            {
                return CreatedByName + " " + CreatedDate; 
            }
        }
    }
}
