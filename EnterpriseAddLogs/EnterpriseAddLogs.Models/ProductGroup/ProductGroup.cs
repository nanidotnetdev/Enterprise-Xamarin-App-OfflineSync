namespace EnterpriseAddLogs.Models
{
    using Newtonsoft.Json;
    using System;

    public class ProductGroupEntity
    {
        [JsonProperty("Id")]
        public Guid ProductGroupId { get; set; }

        public string GroupName { get; set; }

        public char? ProductLine { get; set; }

        public bool IsActive { get; set; }

        public string ImageName { get; set; }

        public string Abbreviation { get; set; }

    }
}