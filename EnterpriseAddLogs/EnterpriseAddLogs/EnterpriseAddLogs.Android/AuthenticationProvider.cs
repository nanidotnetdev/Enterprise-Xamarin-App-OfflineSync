using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Webkit;
using EnterpriseAddLogs.Droid;
using EnterpriseAddLogs.Services;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationProvider))]
namespace EnterpriseAddLogs.Droid
{
    public class AuthenticationProvider : IAuthenticate
    {
        public AccountStore AccountStore { get; private set; }

        public AuthenticationProvider()
        {
            AccountStore = AccountStore.Create(MainActivity.Instance, "passcode");
        }

        public async Task<bool> AuthenticateAsync()
        {
            bool success = false;
            try
            {
                var accounts = AccountStore.FindAccountsForService("enterprisepoc");
                if (accounts != null)
                {
                    foreach (var acct in accounts)
                    {
                        string token;

                        if (acct.Properties.TryGetValue("token", out token))
                        {
                            if (!IsTokenExpired(token))
                            {
                                AzureOfflineService.Client.CurrentUser = new MobileServiceUser(acct.Username);
                                AzureOfflineService.Client.CurrentUser.MobileServiceAuthenticationToken = token;
                                return true;
                            }
                        }
                    }
                }

                //client flow --recommended
                //Server Flow 
                await AzureOfflineService.Client.LoginAsync(MainActivity.Instance, MobileServiceAuthenticationProvider.Facebook, ServiceConstants.Urls.URLScheme);

                if (AzureOfflineService.Client.CurrentUser != null)
                {
                    // Store the new token within the store
                    var account = new Account(AzureOfflineService.Client.CurrentUser.UserId);
                    account.Properties.Add("token", AzureOfflineService.Client.CurrentUser.MobileServiceAuthenticationToken);
                    AccountStore.Save(account, "enterprisepoc");

                    CreateAndShowDialog(string.Format("You are now logged in - {0}", AzureOfflineService.Client.CurrentUser.UserId), "Logged in!");
                }

                success = true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex.Message, "Authentication failed");
            }

            return success;
        }

        bool IsTokenExpired(string token)
        {
            // Get just the JWT part of the token (without the signature).
            var jwt = token.Split(new Char[] { '.' })[1];

            // Undo the URL encoding.
            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch (jwt.Length % 4)
            {
                case 0: break;
                case 2: jwt += "=="; break;
                case 3: jwt += "="; break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            // Convert to a JSON String
            var bytes = Convert.FromBase64String(jwt);
            string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            // Parse as JSON object and get the exp field value,
            // which is the expiration date as a JavaScript primative date.
            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // Calculate the expiration by adding the exp value (in seconds) to the
            // base date of 1/1/1970.
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }

        public async Task LogoutAsync()
        {
            MobileServiceClient client = AzureOfflineService.Client;

            if (client.CurrentUser == null || client.CurrentUser.MobileServiceAuthenticationToken == null)
                return;

            // Log out of the identity provider (if required)

            // Invalidate the token on the mobile backend
            var authUri = new Uri($"{client.MobileAppUri}/.auth/logout");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", client.CurrentUser.MobileServiceAuthenticationToken);
                await httpClient.GetAsync(authUri);
            }

            RemoveTokenFromSecureStore();

            // Remove the token from the MobileServiceClient
            await client.LogoutAsync();
        }

        public void RemoveTokenFromSecureStore()
        {
            var accounts = AccountStore.FindAccountsForService("enterprisepoc");
            if (accounts != null)
            {
                foreach (var acct in accounts)
                {
                    AccountStore.Delete(acct, "enterprisepoc");
                }
            }
        }
        //public async Task<bool> LogoutAsync()
        //{
        //    bool success = false;
        //    try
        //    {
        //        if (user != null)
        //        {
        //            CookieManager.Instance.RemoveAllCookie();
        //            //await LogService.DefaultManager.CurrentClient.LogoutAsync();
        //            CreateAndShowDialog(string.Format("You are now logged out - {0}", user.UserId), "Logged out!");
        //        }
        //        user = null;
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        CreateAndShowDialog(ex.Message, "Logout failed");
        //    }

        //    return success;
        //}

        void CreateAndShowDialog(string message, string title)
        {
            var builder = new AlertDialog.Builder(MainActivity.Instance);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetNeutralButton("OK", (sender, args) => { });
            builder.Create().Show();
        }
    }
}
