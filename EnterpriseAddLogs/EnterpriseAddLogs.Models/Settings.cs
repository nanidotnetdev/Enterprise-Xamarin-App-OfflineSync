﻿using System;

namespace EnterpriseAddLogs.Models
{
    public class Settings
    {
        public string Option { get; set; }

        public bool IsEnabled { get; set; }

        public string HelpText { get; set; }

        public Action<bool> OnChange { get; set; }
    }
}
