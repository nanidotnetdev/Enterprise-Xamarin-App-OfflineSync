using Newtonsoft.Json;

namespace EnterpriseAddLogs.Models
{
    public class User: BaseModel
    {
        //[JsonProperty("id")]
        //public Guid UserId { get; set; }

        /// <summary>
        /// external provider identifier
        /// facebook for now
        /// </summary>
        public string AuthId { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}"; 

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
