using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EnterpriseAddLogs.Models
{
    public class UserClaim
    {
        [JsonProperty(PropertyName = "typ")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "val")]
        public string Value { get; set; }
    }
}
