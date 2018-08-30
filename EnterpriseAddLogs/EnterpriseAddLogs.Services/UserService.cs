namespace EnterpriseAddLogs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EnterpriseAddLogs.Models;
    using Newtonsoft.Json;

    public class UserService : Service, IUserService
    {
        public async Task<UserEntity> GetUserAsync(Guid userId)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, string.Format(ServiceConstants.Urls.GetUserEntity, userId)))
                {

                    AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

                    HttpResponseMessage response = null;

                    using (var cancellationSource = new CancellationTokenSource())
                    {
                        cancellationSource.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

                        response = await client.SendAsync(request, cancellationSource.Token).ConfigureAwait(false);
                    }

                    response.EnsureSuccessStatusCode();

                    var jsonString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<UserEntity>(jsonString);
                }
            }
        }

        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<UserEntity>> GetAllUsersAsync()
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, ServiceConstants.Urls.GetAllUserEntities))
                {
                    AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

                    HttpResponseMessage response = null;

                    using (var cancellationToken = new CancellationTokenSource())
                    {
                        cancellationToken.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

                        response = await client.SendAsync(request, cancellationToken.Token).ConfigureAwait(false);
                    }

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ICollection<UserEntity>>(json);

                    return result;
                }
            }
        }

        /// <summary>
        /// Save User Entity.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<UserEntity> SaveUserAsync(UserEntity user)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, ServiceConstants.Urls.SAveUserEntity))
                {
                    AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

                    HttpResponseMessage response = null;

                    //Instead of POST Async add content in the request.
                    request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"); ;

                    using (var cancellationToken = new CancellationTokenSource())
                    {
                        cancellationToken.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

                        response = await client.SendAsync(request, cancellationToken.Token)
                            .ConfigureAwait(false);

                    }

                    response.EnsureSuccessStatusCode();

                    var jsonString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<UserEntity>(jsonString);
                }
            }
        }

        /// <summary>
        /// Delete User Entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> DeleteUserAsync(Guid id)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, string.Format(ServiceConstants.Urls.DeleteUserEntity, id)))
                {
                    AddRequestHeaders(request, ServiceConstants.SubscriptionKeys.PrimaryKey);

                    HttpResponseMessage response = null;

                    using (var cancellationToken = new CancellationTokenSource())
                    {
                        cancellationToken.CancelAfter(TimeSpan.FromSeconds(ServiceConstants.Defaults.ServiceTimeoutInSecs));

                        response = await client.SendAsync(request, cancellationToken.Token)
                            .ConfigureAwait(false);
                    }

                    response.EnsureSuccessStatusCode();

                    return response.StatusCode;
                }
            }
        }
    }
}
