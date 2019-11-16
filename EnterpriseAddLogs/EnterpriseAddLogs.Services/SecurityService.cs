using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EnterpriseAddLogs.Models;
using Plugin.Connectivity;
using Xamarin.Essentials;

namespace EnterpriseAddLogs.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUserService _userService;

        public SecurityService(IUserService userService)
        {
            this._userService = userService;
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task LoadUserIdentity()
        {
            if (!CrossConnectivity.Current.IsConnected)
                return;

            var identity = await GetIdentityAsync();

            if (identity != null)
            {
                var fbUser = new UserIdentity
                {
                    UserId = identity.UserClaims
                        .FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
                        ?.Value,
                    FullName = identity.UserClaims
                        .FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"))
                        ?.Value,
                    FirstName = identity.UserClaims
                        .FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"))
                        ?.Value,
                    LastName = identity.UserClaims
                        .FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"))
                        ?.Value,
                    Email = identity.UserClaims
                        .FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))
                        ?.Value
                };

                await SyncUserData(fbUser);
            }
        }

        /// <summary>
        /// check if the user has account already
        /// if not create new account.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task SyncUserData(UserIdentity user)
        {
            var users = await _userService.GetItemsAsync(true);
            var existingUser = users.FirstOrDefault(u => u.AuthId.ToLower()
                                                         == user.UserId.ToLower());

            var userId = string.Empty;

            if (existingUser != null)
            {
                //sync the data if the auth provider details changed.
                //if (existingUser.FirstName != user.FirstName
                //    || existingUser.LastName != user.LastName
                //    || existingUser.Email!= user.Email)
                //{
                //    existingUser.FirstName = user.FirstName;
                //    existingUser.LastName = user.LastName;
                //    existingUser.Email = user.Email;

                //    await _userService.UpdateAsync(existingUser);
                //}

                userId = existingUser.Id;
            }
            else
            {
                //new user
                //create new Account
                User newUser = new User();

                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.AuthId = user.UserId;
                newUser.Email = user.Email;
                newUser.Id = Guid.NewGuid().ToString();

                await _userService.InsertAsync(newUser);

                userId = newUser.Id;
            }

            var loggedInUser = await _userService.GetItemAsync(userId);

            if (loggedInUser != null)
                CurrentUser = loggedInUser;
        }

        /// <summary>
        /// Get User Details.
        /// </summary>
        /// <returns></returns>
        private async Task<AppServiceIdentity> GetIdentityAsync()
        {
            if (AppService.Instance.Client.CurrentUser == null || AppService.Instance.Client.CurrentUser?.MobileServiceAuthenticationToken == null)
            {
                throw new InvalidOperationException("Not Authenticated");
            }

            var identities = await AppService.Instance.Client.InvokeApiAsync<List<AppServiceIdentity>>("/.auth/me");

            if (identities.Count > 0)
                return identities[0];

            return null;
        }
    }
}