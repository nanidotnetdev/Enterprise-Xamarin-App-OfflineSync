using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.Mobile.Server;

namespace EnterpriseAddLogs.Backend.DataObjects
{
    [Table("User")]
    public class User:EntityData
    {
        public string AuthId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}