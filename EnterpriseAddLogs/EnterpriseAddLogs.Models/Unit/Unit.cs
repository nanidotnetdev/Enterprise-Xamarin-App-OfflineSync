namespace EnterpriseAddLogs.Models
{
    using System;

    public class UnitEntity
    {
        public Guid UnitID { get; set; }

        public string UnitNumber { get; set; }

        public Guid ProductGroupID { get; set; }

        public string Comment { get; set; }

        public string VIN { get; set; }

        public DateTime? SetupDate { get; set; }

        public Guid? ModelYearID { get; set; }
        public Guid? TestTypeID { get; set; }
        public Guid? LocationID { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? DispositionID { get; set; }
        public Guid? ModelTypeID { get; set; }
        public Guid? PDPPhaseID { get; set; }
        public Guid? EngineID { get; set; }

        public double? EngineHours { get; set; }
        public double? TotalMiles { get; set; }
        public double? TotalCycles { get; set; }
        public double? TotalHours { get; set; }

        public string TestCycles { get; set; }
        public string BIN { get; set; }

        public Guid? DynoID { get; set; }

        public string ImageName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public int? UnitTypeID { get; set; }

        public string EngineVIN { get; set; }

        public double? ProposedHours { get; set; }
        public double? ProposedMiles { get; set; }
        public double? ProposedCycles { get; set; }
    }
}