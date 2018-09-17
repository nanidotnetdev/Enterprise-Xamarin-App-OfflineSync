namespace EnterpriseAddLogs.Models
{
    using Newtonsoft.Json;
    using System;

    public class UserEntity
    {
        [JsonProperty("Id")]
        public Guid UserId { get; set; }

        public int? MemberShipUserID { get; set; }

        public string Username { get; set; }

        public string EmpName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsContractor { get; set; }

        public Guid? RoleID { get; set; }

        public string ProfilePhotoID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
}