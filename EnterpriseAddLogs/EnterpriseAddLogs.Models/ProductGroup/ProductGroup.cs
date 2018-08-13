namespace EnterpriseAddLogs.Models
{
    using System;

    public class ProductGroupEntity
    {
        public Guid Id { get; set; }

        public string GroupName { get; set; }

        public char? ProductLine { get; set; }

        public bool IsActive { get; set; }

        public string ImageName { get; set; }

        public string Abbreviation { get; set; }

    }
}