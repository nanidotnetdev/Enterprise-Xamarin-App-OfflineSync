namespace EnterpriseAddLogs.Models
{
    using System;

    public class LogEntity
    {
        public Guid LogID { get; set; }

        public int LogNumber { get; set; }

        public Guid ProductGroupID { get; set; }

        public Guid? StatusID { get; set; }

        public Guid LogTypeID { get; set; }

        public Guid UnitID { get; set; }

        public Guid EnteredBy { get; set; }

        public DateTime EnteredDate { get; set; }

        public Guid? AssignedDriver { get; set; }

        public int? StartingCycle { get; set; }

        public int? EndingCycle { get; set; }

        public int? CycleRun { get; set; }

        public double? StartingMileage { get; set; }

        public double? EndingMileage { get; set; }

        public double? MilesDriven { get; set; }

        public double? StartingTime { get; set; }

        public double? EndingTime { get; set; }

        public double? HoursRun { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTime? RejectedDate { get; set; }

        public string RejectReason { get; set; }

        public Guid? RejectedBy { get; set; }
        public Guid? SystemStatusID { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public string MetaTag { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}