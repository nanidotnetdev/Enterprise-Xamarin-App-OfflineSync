using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EnterpriseAddLogs.Models
{
    public class AppServiceIdentity
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "user_claims")]
        public List<UserClaim> UserClaims { get; set; }
    }
}
