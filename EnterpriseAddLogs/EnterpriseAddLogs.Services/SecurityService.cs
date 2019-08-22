using System;
using EnterpriseAddLogs.Models;
using Xamarin.Essentials;

namespace EnterpriseAddLogs.Services
{
    class SecurityService: ISecurityService
    {
        public User CurrentUser
        {
            get
            {
                if (Preferences.Get("user_Id", string.Empty) == string.Empty)
                    return null;

                return new User
                {
                    Id = Preferences.Get("user_Id", "0"),
                    AuthId = Preferences.Get("user_AuthId", "0"),
                    Email = Preferences.Get("user_Email", "0"),
                    FirstName = Preferences.Get("user_FirstName", "0"),
                    LastName = Preferences.Get("user_LastName", "0")
                };
            }
            set
            {
                Preferences.Set("user_Id", value.Id);
                Preferences.Set("user_AuthId", value.AuthId);
                Preferences.Set("user_Email", value.Email);
                Preferences.Set("user_FirstName", value.FirstName);
                Preferences.Set("user_LastName", value.LastName);
            }
        }

        public string CurrentUserId => CurrentUser.Id;
    }
}