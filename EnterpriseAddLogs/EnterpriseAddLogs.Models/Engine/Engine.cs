namespace EnterpriseAddLogs.Models
{
    using System;

    public class EngineEntity
    {
        public Guid EngineID { get; set; }

        public string EngineNumber { get; set; }

        public Guid? ProductGroupID { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}