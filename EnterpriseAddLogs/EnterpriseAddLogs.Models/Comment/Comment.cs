using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseAddLogs.Models
{
    public class CommentEntity
    {
        public Guid CommentId { get; set; }

        public Guid EntityItemId { get; set; }

        public Guid EntityTypeId { get; set; }

        public string Comment { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedByName { get; set; }
    }
}
